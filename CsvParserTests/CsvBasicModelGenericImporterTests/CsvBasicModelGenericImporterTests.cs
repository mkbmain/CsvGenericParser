using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using tobin.CsvBasicModelGenericImporter;
using Xunit;

namespace CsvParserTests.CsvBasicModelGenericImporterTests
{
    public class CsvBasicModelGenericImporterTests
    {
        public class TestParse
        {
            public int Id { get; set; }
            public int? OptionalInt { get; set; }
            public string RandomString { get; set; }
            public DateTime? OptionalDate { get; set; }
            public DateTime RequiredDate { get; set; }
            public decimal RequiredDecimal { get; set; }
            public decimal? OptionalDecimal { get; set; }
            public long RequiredLong { get; set; }
            public long? OptionalLong { get; set; }
        }

        private string[] TestParseArray = new[] {"Id", "OptionalInt", "RandomString", "OptionalDate", "RequiredDate", "RequiredDecimal", "OptionalDecimal", "RequiredLong", "OptionalLong"};

        [Fact]
        public async Task Ensure_we_can_parse()
        {
            var basicModel = new TestParse
            {
                Id = 453,
                OptionalInt = 43,
                RandomString = "test",
                OptionalDate = new DateTime(2000, 01, 05),
                RequiredDate = new DateTime(2001, 10, 01),
                RequiredLong = long.MaxValue,
                OptionalLong = long.MaxValue - 1,
                OptionalDecimal = 2001.43m,
                RequiredDecimal = 32.322m
            };

            var contents = $"{Environment.NewLine}{basicModel.Id}|{basicModel.OptionalInt}|{basicModel.RandomString}|{basicModel.OptionalDate}|{basicModel.RequiredDate}|{basicModel.RequiredDecimal}|{basicModel.OptionalDecimal}|{basicModel.RequiredLong}|{basicModel.OptionalLong}";
            var mStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(contents));
            var result = await CsvBasicModelGenericImporter.ImportData<TestParse>(mStream, TestParseArray, '|');
            
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(basicModel.Id);
            result.First().OptionalInt.ShouldBe(basicModel.OptionalInt);
            result.First().RandomString.ShouldBe(basicModel.RandomString);
            result.First().OptionalDate.ShouldBe(basicModel.OptionalDate);
            result.First().RequiredDate.ShouldBe(basicModel.RequiredDate);
            result.First().RequiredLong.ShouldBe(basicModel.RequiredLong);
            result.First().OptionalLong.ShouldBe(basicModel.OptionalLong);
            result.First().OptionalDecimal.ShouldBe(basicModel.OptionalDecimal);
            result.First().RequiredDecimal.ShouldBe(basicModel.RequiredDecimal);
        }
        

        
        [Fact]
        public async Task Ensure_optional_decimal_is_not_required()
        {
            var basicModel = new TestParse
            {
                Id = 453,
                OptionalInt = 43,
                RandomString = "test",
                OptionalDate = new DateTime(2000, 01, 05),
                RequiredDate = new DateTime(2001, 10, 01),
                RequiredLong = long.MaxValue,
                OptionalLong = long.MaxValue - 1,
            //    OptionalDecimal = 2001.43m,
                RequiredDecimal = 32.322m
            };

            var contents = $"{Environment.NewLine}{basicModel.Id}|{basicModel.OptionalInt}|{basicModel.RandomString}|{basicModel.OptionalDate}|{basicModel.RequiredDate}|{basicModel.RequiredDecimal}||{basicModel.RequiredLong}|{basicModel.OptionalLong}";
            var mStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(contents));
            var result = await CsvBasicModelGenericImporter.ImportData<TestParse>(mStream, TestParseArray, '|');
            
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(basicModel.Id);
            result.First().OptionalInt.ShouldBe(basicModel.OptionalInt);
            result.First().RandomString.ShouldBe(basicModel.RandomString);
            result.First().OptionalDate.ShouldBe(basicModel.OptionalDate);
            result.First().RequiredDate.ShouldBe(basicModel.RequiredDate);
            result.First().RequiredLong.ShouldBe(basicModel.RequiredLong);
            result.First().OptionalLong.ShouldBe(basicModel.OptionalLong);
            result.First().OptionalDecimal.ShouldBeNull();
            result.First().RequiredDecimal.ShouldBe(basicModel.RequiredDecimal);
        }
        [Fact]
        public async Task Ensure_Optional_long_is_not_required()
        {
            var basicModel = new TestParse
            {
                Id = 453,
                OptionalInt = 43,
                RandomString = "test",
                OptionalDate = new DateTime(2000, 01, 05),
                RequiredDate = new DateTime(2001, 10, 01),
                RequiredLong = long.MaxValue,
               // OptionalLong = long.MaxValue - 1,
                OptionalDecimal = 2001.43m,
                RequiredDecimal = 32.322m
            };

            var contents = $"{Environment.NewLine}{basicModel.Id}|{basicModel.OptionalInt}|{basicModel.RandomString}|{basicModel.OptionalDate}|{basicModel.RequiredDate}|{basicModel.RequiredDecimal}|{basicModel.OptionalDecimal}|{basicModel.RequiredLong}|";
            var mStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(contents));
            var result = await CsvBasicModelGenericImporter.ImportData<TestParse>(mStream, TestParseArray, '|');
            
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(basicModel.Id);
            result.First().OptionalInt.ShouldBe(basicModel.OptionalInt);
            result.First().RandomString.ShouldBe(basicModel.RandomString);
            result.First().OptionalDate.ShouldBe(basicModel.OptionalDate);
            result.First().RequiredDate.ShouldBe(basicModel.RequiredDate);
            result.First().RequiredLong.ShouldBe(basicModel.RequiredLong);
            result.First().OptionalLong.ShouldBeNull();
            result.First().OptionalDecimal.ShouldBe(basicModel.OptionalDecimal);
            result.First().RequiredDecimal.ShouldBe(basicModel.RequiredDecimal);
        }
        
        [Fact]
        public async Task Ensure_optional_date_is_not_required()
        {
            var basicModel = new TestParse
            {
                Id = 453,
                OptionalInt = 43,
                RandomString = "test",
               // OptionalDate = new DateTime(2000, 01, 05),
                RequiredDate = new DateTime(2001, 10, 01),
                RequiredLong = long.MaxValue,
                OptionalLong = long.MaxValue - 1,
                OptionalDecimal = 2001.43m,
                RequiredDecimal = 32.322m
            };

            var contents = $"{Environment.NewLine}{basicModel.Id}|{basicModel.OptionalInt}|{basicModel.RandomString}||{basicModel.RequiredDate}|{basicModel.RequiredDecimal}|{basicModel.OptionalDecimal}|{basicModel.RequiredLong}|{basicModel.OptionalLong}";
            var mStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(contents));
            var result = await CsvBasicModelGenericImporter.ImportData<TestParse>(mStream, TestParseArray, '|');
            
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(basicModel.Id);
            result.First().OptionalInt.ShouldBe(basicModel.OptionalInt);
            result.First().RandomString.ShouldBe(basicModel.RandomString);
            result.First().OptionalDate.ShouldBeNull();
            result.First().RequiredDate.ShouldBe(basicModel.RequiredDate);
            result.First().RequiredLong.ShouldBe(basicModel.RequiredLong);
            result.First().OptionalLong.ShouldBe(basicModel.OptionalLong);
            result.First().OptionalDecimal.ShouldBe(basicModel.OptionalDecimal);
            result.First().RequiredDecimal.ShouldBe(basicModel.RequiredDecimal);
        }
        
        [Fact]
        public async Task Ensure_if_we_are_missing_a_field_we_throw_data_miss_match()
        {
            var basicModel = new TestParse
            {
                Id = 453,
                OptionalInt = 43,
                RandomString = "test",
                OptionalDate = new DateTime(2000, 01, 05),
                RequiredDate = new DateTime(2001, 10, 01),
                RequiredLong = long.MaxValue,
                OptionalLong = long.MaxValue - 1,
                OptionalDecimal = 2001.43m,
                RequiredDecimal = 32.322m
            };

            var contents = $"{Environment.NewLine}{basicModel.OptionalInt}|{basicModel.RandomString}|{basicModel.OptionalDate}|{basicModel.RequiredDate}|{basicModel.RequiredDecimal}|{basicModel.OptionalDecimal}|{basicModel.RequiredLong}|{basicModel.OptionalLong}";
            var mStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(contents));
            var result = await CsvBasicModelGenericImporter.ImportData<TestParse>(mStream, TestParseArray, '|').ShouldThrowAsync<DataException>();
         result.Message.ShouldBe("Filed Length Miss Match");   
        }
        
        [Fact]
        public async Task Ensure_OptionalIntCanBeNull()
        {
            var basicModel = new TestParse
            {
                Id = 453,
                OptionalInt = null,
                RandomString = "test",
                OptionalDate = new DateTime(2000, 01, 05),
                RequiredDate = new DateTime(2001, 10, 01),
                RequiredLong = long.MaxValue,
                OptionalLong = long.MaxValue - 1,
                OptionalDecimal = 2001.43m,
                RequiredDecimal = 32.322m
            };

            var contents = $"{Environment.NewLine}{basicModel.Id}||{basicModel.RandomString}|{basicModel.OptionalDate}|{basicModel.RequiredDate}|{basicModel.RequiredDecimal}|{basicModel.OptionalDecimal}|{basicModel.RequiredLong}|{basicModel.OptionalLong}";
            var mStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(contents));
            var result = await CsvBasicModelGenericImporter.ImportData<TestParse>(mStream, TestParseArray, '|');
            
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(basicModel.Id);
            result.First().OptionalInt.ShouldBeNull();
            result.First().RandomString.ShouldBe(basicModel.RandomString);
            result.First().OptionalDate.ShouldBe(basicModel.OptionalDate);
            result.First().RequiredDate.ShouldBe(basicModel.RequiredDate);
            result.First().RequiredLong.ShouldBe(basicModel.RequiredLong);
            result.First().OptionalLong.ShouldBe(basicModel.OptionalLong);
            result.First().OptionalDecimal.ShouldBe(basicModel.OptionalDecimal);
            result.First().RequiredDecimal.ShouldBe(basicModel.RequiredDecimal);
        }
    }
}