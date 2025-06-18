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
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,

        // Keywords
        TrueKeyword,
        FalseKeyword
    }
}