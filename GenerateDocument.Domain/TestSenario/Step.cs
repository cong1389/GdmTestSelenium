using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateDocument.Domain.TestSenario
{
    public class Step
    {
        private string _controlValue;
        private CustomValueStep _customValueStep;

        public string ControlId { get; set; }

        public string ControlValue
        {
            get
            {
                if (CustomValueStep != null)
                {
                    string currentType = CustomValueStep.FormatType.ToLower();

                    switch (currentType)
                    {
                        case "random":
                            _controlValue = $"{_controlValue} {RandomString(_customValueStep.Length)}";
                            break;

                        case "appendprefix":
                            _controlValue = $"{_customValueStep.Value} {_controlValue}";
                            break;
                    }
                }

                return _controlValue;
            }
            set { _controlValue = value; }
        }

        public string ControlType { get; set; }

        public string Action { get; set; }

        public CustomValueStep CustomValueStep
        {
            get { return _customValueStep; }
            set { _customValueStep = value; }
        }

        public List<Expectation> Expectations { get; set; }

        public Step()
        {
            Expectations = new List<Expectation>();
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
    }
}
