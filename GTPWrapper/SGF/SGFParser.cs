using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GTPWrapper.SGF {
    /// <summary>
    /// Provides methods to parse a SGF string.
    /// </summary>
    public static class SGFParser {
        /// <summary>
        /// Token types of a SGF string.
        /// </summary>
        public enum TokenType {
            Parenthesis,
            Semicolon,
            PropIdent,
            CValueType
        }

        /// <summary>
        /// Create the token list of a SGF string.
        /// </summary>
        /// <param name="input">The SGF string.</param>
        public static IEnumerable<Tuple<TokenType, string>> Tokenize(string input) {
            StringBuilder builder = new StringBuilder();
            int i = 0;

            bool inCValueType = false;
            bool inBackslash = false;

            while (i < input.Length) {
                if (!inCValueType) {
                    builder.Clear();
                    inBackslash = false;

                    switch (input[i]) {
                        case '(':
                            yield return Tuple.Create(TokenType.Parenthesis, "(");
                            i++;
                            break;
                        case ')':
                            yield return Tuple.Create(TokenType.Parenthesis, ")");
                            i++;
                            break;
                        case ';':
                            yield return Tuple.Create(TokenType.Semicolon, ";");
                            i++;
                            break;
                        case '[':
                            inCValueType = true;
                            i++;
                            break;
                        default:
                            if (char.IsWhiteSpace(input[i])) {
                                i++;
                                break;
                            }

                            Regex propIdentRegex = new Regex(@"[A-Z]+");
                            Match m = propIdentRegex.Match(input, i);

                            if (m.Success && m.Index == i) {
                                yield return Tuple.Create(TokenType.PropIdent, m.Value);
                                i += m.Length;
                            } else {
                                throw new SGFParseException("Unexpected token '" + input[i] + "'");
                            }
                            break;
                    }
                } else {
                    switch (input[i]) {
                        case '\\':
                            if (!inBackslash) {
                                inBackslash = true;
                                i++;
                                break;
                            } else {
                                goto default;
                            }
                        case '\n':
                            if (!inBackslash) {
                                goto default;
                            } else {
                                i++;
                                break;
                            }
                        case ']':
                            if (!inBackslash) {
                                inCValueType = false;
                                yield return Tuple.Create(TokenType.CValueType, builder.ToString());
                                i++;
                                break;
                            } else {
                                goto default;
                            }
                        default:
                            inBackslash = false;
                            builder.Append(input[i]);
                            i++;
                            break;
                    }
                }
            }
        }
    }
}
