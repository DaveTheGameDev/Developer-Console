using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace DeveloperConsole.Tests
{
    public class DeveloperConsole_Tests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void DeveloperConsole_TestsSimplePasses()
        {
            ConsoleSystem.Initialize(new UnityConsole());
            // Use the Assert class to test conditions
            ConsoleSystem.ExecuteCommand("test");
            ConsoleSystem.ExecuteCommand("test-args 1 0 999 test");
        }

        [ConsoleCommand("test", "")]
        private static void TestCommand()
        {
            ConsoleSystem.Log("Test Command Ran");
        }
        
        [ConsoleCommand("test-args", "")]
        private static void TestCommandArgs(bool testBool, bool testBool2, int testInt, string testString)
        {
            Assert.IsTrue(testBool);
            Assert.IsFalse(testBool2);
            Assert.AreEqual("test", testString);
            Assert.AreEqual(999, testInt);
            
            ConsoleSystem.Log($"testInt is {testInt}");
            ConsoleSystem.Log("Test Command Args Ran");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
//        [UnityTest]
//        public IEnumerator DeveloperConsole_TestsWithEnumeratorPasses()
//        {
//            // Use the Assert class to test conditions.
//            // Use yield to skip a frame.
//            yield return null;
//        }
    }
}
