namespace BLToolkit.DataAccess
{
    public class FullSqlQueryInfo : SqlQueryInfo
    {
        public override SqlQueryParameterInfo AddParameter(string parameterName, string fieldName)
        {
            var parameter = new FullSqlQueryParameterInfo { ParameterName = parameterName, FieldName = fieldName };

            parameter.SetMemberMapper(ObjectMapper);

            Parameters.Add(parameter);

            return parameter;
        }
    }
}