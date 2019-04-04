using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task3.EventArgs;

namespace Task3.Tests
{
    [TestClass]
    public class FileSystemFilterTest
    {
        private Mock<FileSystemInfo> _fileSystemInfoMock;
        private IFileSystemFilter _fileSystemFilter;

        [TestInitialize]
        public void TestInt()
        {
            _fileSystemInfoMock = new Mock<FileSystemInfo>();
            _fileSystemFilter = new FileSystemFilter();
        }

        [TestMethod]
        public void FilterOut_FilterIsTrue_ReturnNext()
        {
            //Arrange

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                null,
                i => true);

            //Assert
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsFalse_ReturnExclude()
        {
            //Arrange

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                null,
                i => false);

            //Assert
            Assert.AreEqual(result, Action.Exclude);
        }

        [TestMethod]
        public void FilterOut_FilterIsNull_ReturnNext()
        {
            //Arrange

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                null,
                null);

            //Assert
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsNullItemFoundt_ReturnNext_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s, e) => countOfInvokeItemFound++,
                null, 
                null);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueItemFoundt_ReturnNext_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s, e) => countOfInvokeItemFound++,
                null,
                i => true);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsFalseItemFoundt_ReturnExclude_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s, e) => countOfInvokeItemFound++,
                null,
                i => false);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(result, Action.Exclude);
        }

        [TestMethod]
        public void FilterOut_FilterIsNullIFilteredItemFoundt_ReturnNext()
        {
            //Arrange
            int countOfInvokeFilteredItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                (s, e) => countOfInvokeFilteredItemFound++,
                null);

            //Assert
            Assert.AreEqual(0, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueFilteredItemFoundt_ReturnNext_FilteredItemFoundCall()
        {
            //Arrange
            int countOfInvokeFilteredItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                (s, e) => countOfInvokeFilteredItemFound++,
                i => true);

            //Assert
            Assert.AreEqual(1, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsFilteredItemFoundt_ReturnExclude()
        {
            //Arrange
            int countOfInvokeFilteredItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                (s, e) => countOfInvokeFilteredItemFound++,
                i => false);

            //Assert
            Assert.AreEqual(0, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Exclude);
        }      

        [TestMethod]
        public void FilterOut_FilterIsNullItemFoundAndFilteredItemFound_ReturnNext_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;
            int countOfInvokeFilteredItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s, e) => countOfInvokeItemFound++,
                (s, e) =>  countOfInvokeFilteredItemFound++, 
                null);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(0, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueItemFoundAndFilteredItemFound_ReturnNext_ItemFoundCallAndFilteredItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;
            int countOfInvokeFilteredItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s, e) => countOfInvokeItemFound++,
                (s, e) => countOfInvokeFilteredItemFound++,
                i => true);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(1, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsFalseItemFoundAndFilteredItemFound__ReturnExclude_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;
            int countOfInvokeFilteredItemFound = 0;


            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s, e) => countOfInvokeItemFound++,
                (s, e) => countOfInvokeFilteredItemFound++,
                i => false);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(0, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Exclude);
        }

        [TestMethod]
        public void FilterOut_FilterIsNullItemFoundStopSearch__ReturnStopSearch_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s,e) => Act(e, Action.StopSearch, ref countOfInvokeItemFound),
                null,
                null);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(result, Action.StopSearch);
        }

        [TestMethod]
        public void FilterOut_FilterIsNullItemFoundExclude__ReturnExclude_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s,e) => Act(e, Action.Exclude, ref countOfInvokeItemFound),
                null,
                null);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(result, Action.Exclude);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueItemFoundNextAndFilteredItemFoundNext__ReturnNext_ItemFoundCallAndFilteredItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;
            int countOfInvokeFilteredItemFound = 0;

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s,e) => Act(e, Action.Next, ref countOfInvokeItemFound),
                (s, e) => Act(e, Action.Next, ref countOfInvokeFilteredItemFound),
                i => true);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(1, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueItemFoundExcludeAndFilteredItemFoundNext__ReturnExclude_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;
            int countOfInvokeFilteredItemFound = 0;

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s,e) => Act(e, Action.Exclude, ref countOfInvokeItemFound),
                (s, e) => Act(e, Action.Next, ref countOfInvokeFilteredItemFound),
                i => true);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(0, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Exclude);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueItemFoundStopSearchAndFilteredItemFoundNext__ReturnStopSearch_ItemFoundCall()
        {
            //Arrange
            int countOfInvokeItemFound = 0;
            int countOfInvokeFilteredItemFound = 0;

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                (s, e) => Act(e, Action.StopSearch, ref countOfInvokeItemFound),
                (s, e) => Act(e, Action.Next, ref countOfInvokeFilteredItemFound),
                i => true);

            //Assert
            Assert.AreEqual(1, countOfInvokeItemFound);
            Assert.AreEqual(0, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.StopSearch);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueFilteredItemFoundNext__ReturnNext_FilteredItemFoundCall()
        {
            //Arrange
            int countOfInvokeFilteredItemFound = 0;

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                (s, e) => Act(e, Action.Next, ref countOfInvokeFilteredItemFound),
                i => true);

            //Assert
            Assert.AreEqual(1, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.Next);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueFilteredItemFoundExclude__ReturnExclude_FilteredItemFoundCall()
        {
            //Arrange
            int countOfInvokeFilteredItemFound = 0;

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                (s, e) => Act(e, Action.Exclude, ref countOfInvokeFilteredItemFound),
                i => true);

            //Assert
            Assert.AreEqual(result, Action.Exclude);
            Assert.AreEqual(1, countOfInvokeFilteredItemFound);
        }

        [TestMethod]
        public void FilterOut_FilterIsTrueFilteredItemFoundStopSearch__ReturnStopSearch_FilteredItemFoundCall()
        {
            //Arrange
            int countOfInvokeFilteredItemFound = 0;

            //Act
            var result = _fileSystemFilter.FilterOut(_fileSystemInfoMock.Object,
                null,
                (s, e) => Act(e, Action.StopSearch, ref countOfInvokeFilteredItemFound),
                i => true);

            //Assert
            Assert.AreEqual(1, countOfInvokeFilteredItemFound);
            Assert.AreEqual(result, Action.StopSearch);
        }

        private static void Act<T>(FindedEventArgs<T> e, Action action, ref int count) where T : FileSystemInfo
        {
            count++;
            e.Action = action;
        }
    }
}
