using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyStringCalc.Tests
{
    public class CalculatorTests
    {
        //1 Create a simple String calculator with a method signature:

        //———————————————————————
        //int Add(string numbers)
        //———————————————————————

        //The method can take up to two numbers, separated by commas, and will return their sum.
        //for example “” or “1” or “1,2” as inputs.
        //(for an empty string it will return 0)

        [Theory]
        [InlineData("", 0)]
        [InlineData("1", 1)]
        [InlineData("1,2", 3)]
        public void Add_AddsUpToTwoNumbers(string calculation, int expected)
        {
            var sut = new Calculator();

            var result = sut.Add(calculation);

            result.Should().Be(expected);

        }

        //2 Allow the Add method to handle an unknown amount of numbers

        [Theory]
        [InlineData("", 0)]
        [InlineData("1,2,3", 6)]
        [InlineData("10,2,30,4", 46)]
        public void Add_AddsUpToAnyNumberOfNumbers(string calculation, int expected)
        {
            var sut = new Calculator();

            var result = sut.Add(calculation);

            result.Should().Be(expected);

        }

        //3 Allow the Add method to handle new lines between numbers(instead of commas).

        //the following input is ok: “1\n2,3” (will equal 6)

        [Theory]
        [InlineData("", 0)]
        [InlineData("1,2\n3", 6)]
        [InlineData("10\n2\n30,4", 46)]
        public void Add_AddsUpToAnyNumberOfNumbers_WithNewLine(string calculation, int expected)
        {
            var sut = new Calculator();

            var result = sut.Add(calculation);

            result.Should().Be(expected);

        }

        //4 Support different delimiters
        //to change a delimiter, the beginning of the string will contain a separate line that looks like this: 
        //“//[delimiter]\n[numbers…]” for example “//;\n1;2” should return three where the default delimiter is ‘;’ .
        //the first line is optional.all existing scenarios should still be supported

        [Theory]
        [InlineData("", 0)]
        [InlineData("//;\n1;2;3", 6)]
        [InlineData("//;\n10;2;30;4", 46)]
        public void Add_AddsUpToAnyNumberOfNumbers_VariableDelimiters(string calculation, int expected)
        {
            var sut = new Calculator();

            var result = sut.Add(calculation);

            result.Should().Be(expected);

        }

        //5 Calling Add with a negative number will throw an exception “negatives not allowed” - 
        //and the negative that was passed.
        //if there are multiple negatives, show all of them in the exception message.

        [Theory]
        [InlineData("1,2,-3", "-3")]
        [InlineData("//;\n10;-2;30;-4", "-2,-4")]
        public void Add_ShouldThrowException_WhenNegativesAreUsed(string calculation, string negatives)
        {
            var sut = new Calculator();

            Action action = () => sut.Add(calculation);

            action.Should().Throw<Exception>().WithMessage("Negatives not allowed: " + negatives);

        }

        //Numbers bigger than 1000 should be ignored, so adding 2 + 1001 = 2
        [Theory]
        [InlineData("", 0)]
        [InlineData("//;\n1;2;3000", 3)]
        [InlineData("//;\n1;2;1000", 1003)]
        [InlineData("//;\n10;2;30;4,1001", 46)]
        public void Add_IngoreNumbersGreaterThan1000(string calculation, int expected)
        {
            var sut = new Calculator();

            var result = sut.Add(calculation);

            result.Should().Be(expected);

        }

        //Delimiters can be of any length with the following format: “//[delimiter]\n” for example: “//[***]\n1***2***3” should return 6
        [Theory]
        [InlineData("", 0)]
        [InlineData("//;\n1;2;3", 6)]
        [InlineData("//***\n10,30***4", 44)]
        [InlineData("//***\n10***30***4***1001", 44)]
        public void Add_AnyLengthCustomDelimiters(string calculation, int expected)
        {
            var sut = new Calculator();

            var result = sut.Add(calculation);

            result.Should().Be(expected);

        }
        //Allow multiple delimiters like this: “//[delim1][delim2]\n” for example “//[*][%]\n1*2%3” should return 6.
        [Theory]
        [InlineData("//[*][%]\n1*2%3", 6)]
        [InlineData("//[;][*][#]\n10;30#4*1001", 44)]
        public void Add_HandlingMulitipleDelimiters(string calculation, int expected)
        {
            var sut = new Calculator();

            var result = sut.Add(calculation);

            result.Should().Be(expected);

        }

        //make sure you can also handle multiple delimiters with length longer than one char
        [Theory]
        [InlineData("//[**][%]\n1**2%3", 6)]
        [InlineData("//[;][***][##]\n10;30##4***1001", 44)]
        public void Add_HandlingMulitipleDelimiters_WithVariableLength(string calculation, int expected)
        {
            var sut = new Calculator();

            var result = sut.Add(calculation);

            result.Should().Be(expected);

        }
    }
}
