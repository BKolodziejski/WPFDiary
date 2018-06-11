using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfDiary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDiary.Models;

namespace WpfDiary.ViewModels.Tests
{
    [TestClass()]
    public class DiaryViewModelTests
    {
        [TestInitialize]

        public void TestInitialize()
        {
            new PrivateType(typeof(Diary)).SetStaticField("instance", new Diary());
        }
        
        [TestMethod()]
        public void AddsAllEntriesOnCreationTest()
        {
            Diary.Instance.AddEntry("First Title", "First Content", "Tag1, Tag2");
            Diary.Instance.AddEntry("Second Title", "Second Content", "abc");
            Diary.Instance.AddEntry("Third Title", "Third Content", "def");

            DiaryViewModel vm = new DiaryViewModel();

            Assert.AreEqual(3, vm.Entries.Count);
        }

        [TestMethod()]
        public void IsInformedAboutCreationTest()
        {
            Diary.Instance.AddEntry("First Title", "First Content", "Tag1, Tag2");

            DiaryViewModel vm = new DiaryViewModel();

            Assert.AreEqual(1, vm.Entries.Count);

            Diary.Instance.AddEntry("abc", "def", "tag");

            Assert.AreEqual(2, vm.Entries.Count);
        }

        [TestMethod()]
        public void IsInformedAboutDeletionTest()
        {
            new PrivateType(typeof(MainWindow)).SetStaticField("instance", new MainWindow());

            Diary.Instance.AddEntry("First Title", "First Content", "Tag1, Tag2");
            Diary.Instance.AddEntry("abc", "def", "tag");

            DiaryViewModel vm = new DiaryViewModel();

            Assert.AreEqual(2, vm.Entries.Count);
            
            Diary.Instance.RemoveEntry(vm.Entries.First().Entry);

            Assert.AreEqual(1, vm.Entries.Count);
        }

        [TestMethod()]
        public void IsInformedAboutUpdates()
        {
            Diary.Instance.AddEntry("First Title", "First Content", "Tag1, Tag2");
            Diary.Instance.AddEntry("abc", "def", "tag");

            DiaryViewModel vm = new DiaryViewModel();

            Assert.AreEqual(2, vm.Entries.Count);

            Diary.Instance.EditEntry(vm.Entries.First().Entry, "newTitle", "newContent", "tags", DateTime.Now);

            Assert.AreEqual("newTitle", vm.Entries.First().Entry.Title);
        }
    }
}