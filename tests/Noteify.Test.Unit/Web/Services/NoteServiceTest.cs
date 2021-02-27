using System;
using System.Collections.Generic;
using System.Linq;
using Noteify.Data.Models;
using Noteify.Web.Helpers;
using Noteify.Web.Services;
using Xunit;

namespace Noteify.Test.Unit.Web.Services
{
    public class NoteServiceUnitTest
    {
        private readonly List<Note> Notes;

        public NoteServiceUnitTest()
        {
            // Do not change any existing data
            Notes = new List<Note>()
            {
                new Note
                {
                    Designation = "Test",
                    TimeStamp = new DateTime(2020, 04, 15).AddHours(22),
                    IsDeleted = false
                },
                new Note
                {
                    Designation = "Super",
                    TimeStamp = new DateTime(2019, 03, 12).AddHours(16),
                    IsDeleted = false
                },
                new Note{
                    Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Designation = "Note 3000",
                    Message = "This is a super note!",
                    TimeStamp = new DateTime(2019, 03, 12).AddHours(16),
                    IsDeleted = false
                },
                new Note{
                    Id = new Guid("3f5375b588b749a7ba51291cee720b3f"),
                    Designation = "Deleted Note",
                    Message = "This note is deleted.",
                    TimeStamp = new DateTime(2020, 03, 12).AddHours(4),
                    IsDeleted = true
                }
            };
        }

        [Fact]
        public void GetDelegateFromFilterFunction_BothFilterOptionsGivenAndMatches_OneNote()
        {
            // Given
            var matchingDesignation = "Super";
            var matchingDate = new DateTime(2019, 03, 12)
                    .AddHours(16).ToString();

            var filter = new NoteIndexFilter()
            {
                Designation = matchingDesignation,
                Date = matchingDate
            };

            // When
            var delegateResult = NoteService.GetNotesFilter(filter);

            // Then
            var results = Notes.AsQueryable().Where(delegateResult).ToList();

            Assert.Single(results);
            Assert.Equal(filter.Designation, results[0].Designation);
            Assert.Equal(filter.Date, results[0].TimeStamp.ToString());
        }

        [Fact]
        public void GetDelegateFromFilterFunction_BothFilterOptionsGivenOnlyDesignationMatches_Empty()
        {
            // Given
            var matchingDesignation = "Note 3000";
            var mismatchingDate = new DateTime(2019, 12, 31)
                    .AddHours(8).ToString();

            var filter = new NoteIndexFilter()
            {
                Designation = matchingDesignation,
                Date = mismatchingDate
            };

            // When
            var delegateResult = NoteService.GetNotesFilter(filter);

            // Then
            var results = Notes.AsQueryable().Where(delegateResult).ToList();

            Assert.Empty(results);
        }

        [Fact]
        public void GetDelegateFromFilterFunction_OneFilterOptionGivenAndMatches_OneNote()
        {
            // Given
            var matchingDesignation = "Super";
            var emptyDate = "";

            var filter = new NoteIndexFilter()
            {
                Designation = matchingDesignation,
                Date = emptyDate
            };

            // When
            var delegateResult = NoteService.GetNotesFilter(filter);

            // Then
            var results = Notes.AsQueryable().Where(delegateResult).ToList();

            Assert.Single(results);
            Assert.Equal(filter.Designation, results[0].Designation);
        }

        [Fact]
        public void GetDelegateFromFilterFunction_OneFilterOptionGivenAndMatches_TwoNotes()
        {
            // Given
            var emptyDesignation = "";
            var matchingDate = new DateTime(2019, 03, 12)
                .AddHours(16).ToString();

            var filter = new NoteIndexFilter()
            {
                Designation = emptyDesignation,
                Date = matchingDate
            };

            // When
            var delegateResult = NoteService.GetNotesFilter(filter);

            // Then
            var results = Notes.AsQueryable().Where(delegateResult).ToList();

            Assert.Equal(2, results.Count);
            Assert.Equal(filter.Date, results[0].TimeStamp.ToString());
            Assert.Equal(filter.Date, results[1].TimeStamp.ToString());
        }

        [Fact]
        public void GetDelegateFromFilterFunction_NoFilterOptionGiven_AllNotes()
        {
            // Given
            var emptyDesignation = "";
            var emptyDate = "";

            var filter = new NoteIndexFilter()
            {
                Designation = emptyDesignation,
                Date = emptyDate
            };

            // When
            var delegateResult = NoteService.GetNotesFilter(filter);

            // Then
            var results = Notes.AsQueryable().Where(delegateResult).ToList();

            Assert.Equal(3, results.Count);
        }

        [Fact]
        public void GetDelegateFromFilterFunction_DeletedOnlyFilterOptionGiven_AllIsDeletedNotes()
        {
            // Given
            var emptyDesignation = "";
            var emptyDate = "";
            var deletedOnly = true;

            var filter = new NoteIndexFilter()
            {
                Designation = emptyDesignation,
                Date = emptyDate,
                DeletedOnly = deletedOnly
            };

            // When
            var delegateResult = NoteService.GetNotesFilter(filter);

            // Then
            var results = Notes.AsQueryable().Where(delegateResult).ToList();

            Assert.Single(results);
            Assert.Equal("Deleted Note", results[0].Designation);
        }
    }
}
