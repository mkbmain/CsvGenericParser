using System.Linq;
using Shouldly;
using tobin;
using Xunit;

namespace CsvParserTests.ExtensionsTests
{
    public class SmartCsvSplitTests
    {
        [Fact]
        public void ensure_we_remove_quotes_and_ignore_delmiter_only_not_in_quotes()
        {
           var result = "\"hello\",\"boo,\"".SmartCsvSplit(',');
           result.Count().ShouldBe(2);
           result.First().ShouldBe("hello");
           result.Last().ShouldBe("boo,"); // ensure we still got comma
        }
    }
}