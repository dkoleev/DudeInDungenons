using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Runtime.Logic.Core.SaveEngine {
    public class SaveEngine<TProgress> where TProgress : IProgress {
        private TProgress _progress;
        
#if UNITY_EDITOR
        private static readonly string SaveFileName = "save.json";
#else
        private static readonly string SaveFileName = "save.data";
#endif
        private static readonly string BackupFileName = "save.bak";
        private static readonly string NewSaveFileName = "save.new";

        private static readonly string SaveFilePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        private static readonly string BackupSaveFilePath = Path.Combine(Application.persistentDataPath, BackupFileName);
        private static readonly string NewSaveFilePath = Path.Combine(Application.persistentDataPath, NewSaveFileName);
        
        private readonly JsonSerializer _serializer;
        private readonly DESCryptoServiceProvider _cryptic;

        public SaveEngine(TProgress progress) {
            _progress = progress;
            
            _serializer = JsonSerializer.Create(
                new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            _cryptic = new DESCryptoServiceProvider {
                Key = Encoding.UTF8.GetBytes("AvocadoG"),
                IV = Encoding.UTF8.GetBytes("K-Keeper"),
                Padding = PaddingMode.Zeros
            };
        }

        public void SaveProgress() {
            FileStream fileStream = null;
            try {
                fileStream = new FileStream(NewSaveFilePath, FileMode.Create, FileAccess.Write);
#if UNITY_EDITOR
                var writer = new JsonTextWriter(new StreamWriter(fileStream));
#else
                var cryptoStream = new CryptoStream(fileStream, _cryptic.CreateEncryptor(), CryptoStreamMode.Write);
                var writer = new BsonWriter(cryptoStream);
#endif
                _serializer.Serialize(writer, _progress);
                writer.Close();

                if (File.Exists(SaveFilePath)) {
                    if (File.Exists(BackupSaveFilePath)) {
                        File.Delete(BackupSaveFilePath);
                    }

                    File.Move(SaveFilePath, BackupSaveFilePath);
                }

                if (File.Exists(NewSaveFilePath)) {
                    File.Move(NewSaveFilePath, SaveFilePath);
                }

                File.Delete(BackupSaveFilePath);
            } catch (Exception e) {
                Debug.LogError(e);
            } finally {
                fileStream?.Close();
            }
        }

        public TProgress LoadProgress() {
            FileStream fileStream = null;
            try {
                var filePath = SaveFilePath;
                if (!File.Exists(filePath)) {
                    filePath = NewSaveFilePath;
                    if (!File.Exists(filePath)) {
                        filePath = BackupSaveFilePath;
                    }
                }

                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
#if UNITY_EDITOR
                var reader = new JsonTextReader(new StreamReader(fileStream));
#else
                var cryptoStream = new CryptoStream(fileStream, _cryptic.CreateDecryptor(), CryptoStreamMode.Read);
                var reader = new BsonReader(cryptoStream);
#endif

                _progress = _serializer.Deserialize<TProgress>(reader);
                
                if (_progress == null) {
                    throw new Exception("Can't parse save file!");
                }

                return _progress;
                
            } catch (FileNotFoundException) {
                return _progress;
            } catch (Exception e) {
                throw new Exception(e.ToString());
            } finally {
                fileStream?.Close();
            }
        }
    }
}
