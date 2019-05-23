using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GenerateDocument.Domain.Designs;

namespace GenerateDocument.Domain.TestSenario
{
    public class Step
    {
        private string _controlValue;
        private ArgumentModel _argument;

        private MappingModel<DesignModel> _mappingModel;

        public string ControlId { get; set; }

        public string ControlValue
        {
            get
            {
                _controlValue = CustomControlValue(CustomValueStep, _controlValue);
                return _controlValue;
            }
            set { _controlValue = value; }
        }

        public string ControlType { get; set; }

        public string Action { get; set; }

        public CustomValueStep CustomValueStep { get; set; }

        public ArgumentModel Argument
        {
            get => _argument;

            set
            {
                _argument = value;

                var pattern = @"{([A-Za-z0-9\-]+)\(\)}";

                foreach (var prop in typeof(ArgumentModel).GetProperties())
                {
                    var argValue = prop.GetValue(_argument)?.ToString();
                    if (!string.IsNullOrEmpty(argValue))
                    {
                        var matches = Regex.Matches(argValue, pattern, RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            var expressionValue = match.Groups[1].Value;

                            Enum.TryParse(expressionValue, true, out FormatTypes customFormatType);
                            switch (customFormatType)
                            {
                                case FormatTypes.Random:
                                    argValue = Regex.Replace(argValue, pattern, RandomString(5));
                                    break;
                            }

                            prop.SetValue(_argument, argValue);
                        }
                    }
                }

            }
        }

        public MappingModel<DesignModel> MappingModel
        {
            get
            {
                if (_mappingModel == null)
                {
                    return _mappingModel;
                }

                foreach (var propertyInfo in typeof(DesignModel).GetProperties())
                {
                    if (_mappingModel.PropertyName.Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        _mappingModel.PropertyValue = _controlValue;
                    }
                }

                return _mappingModel;
            }

            set => _mappingModel = value;
        }

        public List<Expectation> Expectations { get; }

        public Step()
        {
            Expectations = new List<Expectation>();
        }

        private enum FormatTypes
        {
            Random,
            AppendPrefix,
            Suffixes,
            Argument,
            Expression
        }

        private string RandomString(int size)
        {
            var builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private string CustomControlValue(CustomValueStep customValueStep, string controlValue)
        {
            if (customValueStep == null)
            {
                return controlValue;
            }

            string result = customValueStep.Expression;

            result = SetValueExpressionByPattern(result);
            result = SetValueExpressionByFunctionPattern(result);

            return result;
        }

        private string SetValueExpressionByFunctionPattern(string  input)
        {
            string result = input;

            var patternFn = @"{([A-Za-z0-9\-]+)\(\)}";
            var matches = Regex.Matches(input, patternFn, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                var formatType = match.Groups[1].Value;
                var argValue = SetValueByFormatType(formatType);

                result = Regex.Replace(result, patternFn, argValue);
            }

            return result;
        }

        private string SetValueExpressionByPattern(string input)
        {
            var patternArg = @"{([A-Za-z0-9\-]+)}";

            string result = input;

            var matches = Regex.Matches(input, patternArg, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                var expressionValue = match.Groups[1].Value;

                var argValue = typeof(ArgumentModel).GetProperties()
                    .FirstOrDefault(x => x.Name.Equals(expressionValue, StringComparison.OrdinalIgnoreCase))?.GetValue(Argument).ToString();

                result = Regex.Replace(result, match.Groups[0].Value, argValue);
            }

            return result;
        }

        private string SetValueByFormatType(string formatType)
        {
            string result = string.Empty;

            Enum.TryParse(formatType, true, out FormatTypes customFormatType);

            switch (customFormatType)
            {
                case FormatTypes.Random:
                    result = RandomString(5);
                    break;
            }

            return result;
        }

    }
}
