namespace H3.Data.Filter
{
    internal class ItemMatcher
    {
        private string filed;
        private ComparisonOperatorType comparisonOperatorType;
        private string value;

        public ItemMatcher(string filed, ComparisonOperatorType comparisonOperatorType, string value)
        {
            this.filed = filed;
            this.comparisonOperatorType = comparisonOperatorType;
            this.value = value;
        }
    }
}