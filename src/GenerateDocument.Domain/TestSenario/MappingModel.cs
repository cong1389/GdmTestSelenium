namespace GenerateDocument.Domain.TestSenario
{
    public class MappingModel<T>
    where T : class, new()
    {
        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }
    }
}
