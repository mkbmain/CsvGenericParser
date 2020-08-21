using Shouldly;
using tobin;
using Xunit;

namespace CsvParserTests.ExtensionsTests
{
    public class CamelCaseTests
    {
        [Fact]
        public void Ensure_we_play_correctly_with_underscores()
        {
            "test_be".CamelCase().ShouldBe("TestBe");
        }

        [Fact]
        public void Ensure_we_play_correctly_with_spaces()
        {
            "test be".CamelCase().ShouldBe("TestBe");
        }

        [Fact]
        public void Ensure_we_play_correctly_with_hypons()
        {
            "test-be".CamelCase().ShouldBe("TestBe");
        }

        [Fact]
        public void Ensure_we_play_correctly_with_Capitals()
        {
            "teStbe".CamelCase().ShouldBe("Testbe");
        }


        [Fact]
        public void Ensure_we_can_remove_all_obsticals_and_set_our_own()
        {
            "t- _eStb,e".CamelCase(new[] {','}).ShouldBe("T- _estbE"); // we prove we ignore _ - and spaces if we set it to comma only
        }
    }
}