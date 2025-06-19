namespace Joker.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        LiteralToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        BangToken,
        EqualsToken,
        EqualsEqualsToken,
        BangEqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken,
        IdentifierToken,

        //Expressions
        LiteralExpression,
        NameExpression,
        AssignmentExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,

        // Keywords
        TrueKeyword,
        FalseKeyword
    }
}