namespace EmmaDoesMath;

internal enum Operator
{
    Add,
    Sub,
    Div,
    Mul,
}

internal static class OperatorExtensions
{
    public static string AsString(this Operator op) => op switch
    {
        Operator.Add => "+",
        Operator.Sub => "-",
        Operator.Div => "/",
        Operator.Mul => "x",
        _ => throw new NotImplementedException()
    };
}
