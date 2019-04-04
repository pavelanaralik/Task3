using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Task3.Tests
{
    [TestClass]
    public class FileSystemVisitorTest
    {
        [TestInitialize]
        public void TestInt()
        {
        }

        [TestMethod]
        public void GetFileSystemInfoSequenceDirectoryIsNotExists()
        {
            //Arrange
            var fileSystemVisitor = new FileSystemVisitor();
            var directoryInfo = new DirectoryInfo("Fake");
           
            //Act
            //Assert
            Assert.ThrowsException<DirectoryNotFoundException>(() => fileSystemVisitor.GetFileSystemInfoSequence(directoryInfo).ToList());          
        }      
    }
}
