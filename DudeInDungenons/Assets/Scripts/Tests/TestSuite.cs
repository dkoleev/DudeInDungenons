using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class TestSuite {
        [UnityTest]
        public IEnumerator HelloWorldTest() {
            yield return new WaitForSeconds(1.0f);
            var isSayHello = true;
            Assert.True(isSayHello);
        }
    }
}