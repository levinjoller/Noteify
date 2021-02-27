using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Noteify.Data.Models;
using Noteify.Web.Areas.Account.Helpers;
using Noteify.Web.Areas.Account.Services;
using Xunit;

namespace Noteify.Test.Unit.Web.Services
{
    public class ManageServiceTest
    {
        [Fact]
        public void GetElementQuotedFromQuoteFuncton_GivenString_ReturnStringQuoted()
        {
            //Given
            var unquotedString = "Test";

            //When
            var actual = ManageService.Quote(unquotedString);

            //Then
            Assert.StartsWith("\"", actual);
            Assert.EndsWith("\"", actual);
        }

        [Fact]
        public void GetElementQuotedFromQuoteFuncton_GivenNumber_ReturnNumberQuoted()
        {
            //Given
            var unquotedNum = 123;

            //When
            var actual = ManageService.Quote(unquotedNum);

            //Then
            Assert.StartsWith("\"", actual);
            Assert.EndsWith("\"", actual);
        }

        [Fact]
        public void GetElementQuotedFromQuoteFuncton_GivenStringQuoted_ReturnDoubleQuotedString()
        {
            //Given
            var quotedString = "\"QuotedString\"";

            //When
            var actual = ManageService.Quote(quotedString);

            //Then
            Assert.StartsWith("\"\"", actual);
            Assert.EndsWith("\"\"", actual);
        }

        [Fact]
        public void AddSeparatorAndColumnHeaderToStringBuilder_GivenSeparatorAndHeader_StringBuilderContainsThem()
        {
            //Given
            var exportConfig = new ExportConfig(
                stringBuilder: new StringBuilder(),
                separator: ",",
                columnHeader: new string[] { "H1", "H2" },
                content: null
            );

            //When
            ManageService.AddSeparatorAndColumnHeaderToStringBuilder(exportConfig);
            var actual = exportConfig.StringBuilder.ToString();

            //Then
            Assert.Equal("sep=," + Environment.NewLine + "H1,H2" + Environment.NewLine, actual);
        }

        [Fact]
        public void AddQuotedNotesToStringBuilder_GivenExportConfig_ReturnNotesSeparated()
        {
            //Given
            var exportConfig = new ExportConfig(
                stringBuilder: new StringBuilder(),
                separator: ",",
                columnHeader: null,
                content: new List<Note>()
                {
                    new Note(){
                        Designation = "Note01",
                        Message = "Test",
                        TimeStamp = new DateTime(2021, 04, 23)
                    },
                    new Note(){
                        Designation = "Note02",
                        Message = "Awesome",
                        TimeStamp = new DateTime(2020, 12, 12)
                    },
                }
            );

            //When
            ManageService.AddQuotedNotesToStringBuilder(exportConfig);
            var actual = exportConfig.StringBuilder.ToString();

            //Then
            Assert.Equal(4, actual.Count(x => x == ','));
            Assert.Equal(2, Regex.Matches(actual, Environment.NewLine).Count);
        }
    }
}