using Joker.CodeAnalysis.Syntax;
using Xunit;

namespace Compiler.Tests.CodeAnalysis.Syntax
{
    public sealed class LexerTests
    {
        [Fact]
        public void Lexer_Tests_AllTokens()
        {
            var tokenKinds = Enum.GetValues<SyntaxKind>()
                .Where(k => k.ToString().EndsWith("Keyword") ||
                            k.ToString().EndsWith("Token"));

            var testedTokenKinds = GetTokens().Concat(GetSeparators()).Select(t => t.Kind);
            var untestedTokenKinds = new SortedSet<SyntaxKind>(tokenKinds);

            untestedTokenKinds.Remove(SyntaxKind.BadToken);
            untestedTokenKinds.Remove(SyntaxKind.EndOfFileToken);
            untestedTokenKinds.ExceptWith(testedTokenKinds);

            Assert.Empty(untestedTokenKinds);
        }

        [Theory]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_Lexes_Token(SyntaxKind kind, string text)
        {
            var token = Assert.Single(SyntaxTree.ParseTokens(text));

            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsData))]
        public void Lexer_Lexes_TokenPairs(SyntaxKind firstKind, string firstText,
                                           SyntaxKind secondKind, string secondText)
        {
            var tokens = SyntaxTree.ParseTokens(firstText + secondText).ToArray();

            Assert.Equal(2, tokens.Length);
            Assert.Equal(firstKind, tokens[0].Kind);
            Assert.Equal(firstText, tokens[0].Text);
            Assert.Equal(secondKind, tokens[1].Kind);
            Assert.Equal(secondText, tokens[1].Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsWithSeparatorData))]
        public void Lexer_Lexes_TokenPairs_WithSeparators(SyntaxKind firstKind, string firstText,
                                                           SyntaxKind separatorKind, string separatorText,
                                                           SyntaxKind secondKind, string secondText)
        {
            var tokens = SyntaxTree.ParseTokens(firstText + separatorText + secondText).ToArray();

            Assert.Equal(3, tokens.Length);
            Assert.Equal(firstKind, tokens[0].Kind);
            Assert.Equal(firstText, tokens[0].Text);
            Assert.Equal(separatorKind, tokens[1].Kind);
            Assert.Equal(separatorText, tokens[1].Text);
            Assert.Equal(secondKind, tokens[2].Kind);
            Assert.Equal(secondText, tokens[2].Text);
        }

        public static IEnumerable<object[]> GetTokensData()
        {
            foreach (var token in GetTokens().Concat(GetSeparators()))
                yield return new object[] { token.Kind, token.Text };
        }

        public static IEnumerable<object[]> GetTokenPairsData()
        {
            foreach (var pair in GetTokenPairs())
                yield return new object[] { pair.FirstKind, pair.FirstText, pair.SecondKind, pair.SecondText };
        }

        public static IEnumerable<object[]> GetTokenPairsWithSeparatorData()
        {
            foreach (var pair in GetTokenPairsWithSeparator())
                yield return new object[]
                {
                    pair.FirstKind, pair.FirstText,
                    pair.SeparatorKind, pair.SeparatorText,
                    pair.SecondKind, pair.SecondText
                };
        }

        private static IEnumerable<(SyntaxKind Kind, string Text)> GetTokens()
        {
            var fixedTokens = Enum.GetValues<SyntaxKind>()
                .Select(kind => (Kind: kind, Text: SyntaxFacts.GetText(kind)))
                .Where(token => token.Text is not null)
                .Select(token => (token.Kind, token.Text!));

            var dynamicTokens = new[]
            {
                (SyntaxKind.LiteralToken, "1"),     // Minsk's equivalent is NumberToken
                (SyntaxKind.LiteralToken, "123"),   // Minsk's equivalent is NumberToken
                (SyntaxKind.IdentifierToken, "a"),
                (SyntaxKind.IdentifierToken, "abc")
            };

            return fixedTokens.Concat(dynamicTokens);
        }

        private static IEnumerable<(SyntaxKind Kind, string Text)> GetSeparators()
        {
            return new[]
            {
                (SyntaxKind.WhitespaceToken, " "),
                (SyntaxKind.WhitespaceToken, "  "),
                (SyntaxKind.WhitespaceToken, "\r"),
                (SyntaxKind.WhitespaceToken, "\n"),
                (SyntaxKind.WhitespaceToken, "\r\n")
            };
        }

        private static bool RequiresSeparator(SyntaxKind firstKind, SyntaxKind secondKind)
        {
            var firstIsKeyword = firstKind.ToString().EndsWith("Keyword");
            var secondIsKeyword = secondKind.ToString().EndsWith("Keyword");

            if (firstKind == SyntaxKind.IdentifierToken && secondKind == SyntaxKind.IdentifierToken)
                return true;

            if (firstIsKeyword && secondIsKeyword)
                return true;

            if (firstIsKeyword && secondKind == SyntaxKind.IdentifierToken)
                return true;

            if (firstKind == SyntaxKind.IdentifierToken && secondIsKeyword)
                return true;

            if (firstKind == SyntaxKind.LiteralToken && secondKind == SyntaxKind.LiteralToken)
                return true;

            if (firstKind == SyntaxKind.BangToken && secondKind == SyntaxKind.EqualsToken)
                return true;

            if (firstKind == SyntaxKind.BangToken && secondKind == SyntaxKind.EqualsEqualsToken)
                return true;

            if (firstKind == SyntaxKind.EqualsToken && secondKind == SyntaxKind.EqualsToken)
                return true;

            if (firstKind == SyntaxKind.EqualsToken && secondKind == SyntaxKind.EqualsEqualsToken)
                return true;

            return false;
        }

        private static IEnumerable<(SyntaxKind FirstKind, string FirstText,
                                    SyntaxKind SecondKind, string SecondText)> GetTokenPairs()
        {
            foreach (var first in GetTokens())
            {
                foreach (var second in GetTokens())
                {
                    if (!RequiresSeparator(first.Kind, second.Kind))
                        yield return (first.Kind, first.Text, second.Kind, second.Text);
                }
            }
        }

        private static IEnumerable<(SyntaxKind FirstKind, string FirstText,
                                    SyntaxKind SeparatorKind, string SeparatorText,
                                    SyntaxKind SecondKind, string SecondText)> GetTokenPairsWithSeparator()
        {
            foreach (var first in GetTokens())
            {
                foreach (var second in GetTokens())
                {
                    if (!RequiresSeparator(first.Kind, second.Kind))
                        continue;

                    foreach (var separator in GetSeparators())
                    {
                        yield return (first.Kind, first.Text,
                                      separator.Kind, separator.Text,
                                      second.Kind, second.Text);
                    }
                }
            }
        }
    }
}
