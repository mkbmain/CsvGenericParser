using Shouldly;
using tobin;
using Xunit;

namespace CsvParserTests.ExtensionsTests
{
    public class SanitizeTests
    {
        [Fact]
        public void Ensure_if_no_comma_nothing_changes()
        {
            "523452".Sanitize().ShouldBe("523452");
        }
        [Fact]
        public void Ensure_if_not_all_numbers_nothing_changes()
        {
            "5234,52a".Sanitize().ShouldBe("5234,52a");
        }
        [Fact]
        public void Ensure_comman_are_changed_to_dots_if_all_numbers()
        {
            "5234,52".Sanitize().ShouldBe("5234.52");
        }
    }
}