using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Runtime.Ui {
    public class LoadingScreen : MonoBehaviour {
        [SerializeField]
        private CanvasGroup _title;
        [SerializeField]
        private Transform _backGround;
        [SerializeField]
        private LoadingBar _loadingBar;
        
        private void Awake() {
            _loadingBar.SetBar(0);
            _title.DOFade(0f, 0f);
            _backGround.DOScale(1.05f, 0f);
        }

        IEnumerator Start() {
            _title.DOFade(1f, 0.5f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(0.3f);
            
            _backGround.transform.DOKill();
            _backGround.transform.DOScale(1f, 1.5f).SetEase(Ease.Linear);
            
            yield return StartCoroutine(StartLoading());
            
            yield return null;
        }
        
        IEnumerator StartLoading()
        {
            int maxSize = 64;
            int value = 0;

            _loadingBar.InIt(maxSize);
            _loadingBar.Show();

            while (value <= maxSize)
            {
                _loadingBar.SetBar(value);
                value += 1;
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(1f);
            _loadingBar.Hide();
        }
    }
}