using System;
using System.Collections.Generic;
using System.Text;
using GenerateDocument.Domain.Designs;

namespace GenerateDocument.Domain.TestSenario
{
    public class Step
    {
        private string _controlValue;

        private MappingModel<DesignModel> _mappingModel;

        public string ControlId { get; set; }

        public string ControlValue
        {
            get
            {
                _controlValue = GetControlValueByCustomFormat(CustomValueStep, _controlValue);
                return _controlValue;
            }
            set => _controlValue = value;
        }

        public string ControlType { get; set; }

        public string Action { get; set; }

        public CustomValueStep CustomValueStep { get; set; }

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
            Suffixes
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

        private string GetControlValueByCustomFormat(CustomValueStep customValueStep, string controlValue)
        {
            if (customValueStep == null)
            {
                return controlValue;
            }

            string result = string.Empty;

            Enum.TryParse(customValueStep.FormatType, true, out FormatTypes customFormatType);

            switch (customFormatType)
            {
                case FormatTypes.Random:
                    result = $"{controlValue} {RandomString(customValueStep.Length)}";
                    break;

                case FormatTypes.AppendPrefix:
                    result = $"{customValueStep.Value} {controlValue}";
                    break;

                case FormatTypes.Suffixes:
                    result = $"{controlValue} {RandomString(customValueStep.Length)}";
                    break;
            }

            return result;
        }

    }
}
