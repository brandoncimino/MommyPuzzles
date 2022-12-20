global using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using NUnit.Framework.Constraints;

namespace MommyPuzzles;

/// <summary>
/// Various things to help execute the challenges' tests.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public static class Backstage
{
    private const string CSI = $"\u001B[";
    private const string FG = $"{CSI}0m";
    private const string Red = $"{CSI}1;31m";
    private const string Green = $"{CSI}32m";
    private const string Yellow = $"{CSI}33m";
    private const string Blue = $"{CSI}34m";
    private const string Magenta = $"{CSI}35m";
    private const string Cyan = $"{CSI}36m";
    private const string Reset = $"{CSI}m";
    private const string Bold = $"{CSI}1m";
    private const string MethodColor = Blue;
    private const string ArgColor = Cyan;

    /// <summary>
    /// Evaluates some <paramref name="code"/>, passing in <paramref name="args"/>, and <see cref="Assert"/>s that the result satisfies an NUnit <see cref="IConstraint"/>. 
    /// </summary>
    /// <remarks>
    /// Wraps any non-<see cref="AssertionException"/>s thrown by the <paramref name="code"/> with <see cref="Assert.Fail(string?)"/>, causing "multiple assert" blocks to function as expected.
    /// </remarks>
    /// <param name="code">the code being evaluated</param>
    /// <param name="args">the arguments passed to the <paramref name="code"/>, contained inside of an <see cref="ITuple"/></param>
    /// <param name="constraint">an <see cref="IConstraint"/> that the result of the <paramref name="code"/> must satisfy</param>
    /// <param name="_code">see <see cref="CallerArgumentExpressionAttribute"/></param>
    /// <typeparam name="DELEGATE">a <see cref="Delegate"/> type</typeparam>
    /// <typeparam name="ARGS">an <see cref="ITuple"/> type</typeparam>
    public static void Check<DELEGATE, ARGS>(
        DELEGATE code,
        ARGS args,
        IConstraint constraint,
        [CallerArgumentExpression("code")] string? _code = default
    )
        where DELEGATE : Delegate
        where ARGS : ITuple
    {
        var argArray = args.ToArray();
        var methodString = FormatLambda(_code, argArray);
        try
        {
            var actual = code.DynamicInvoke(argArray);
            Assert.That(actual, constraint, methodString);
            Console.WriteLine($"{Green}🎊{FG} The {methodString} satisfied the constraint {Green}{constraint}{FG}!");
        }
        catch (AssertionException)
        {
            constraint.ApplyTo(5);
            // AssertionExceptions will already be intercepted by the "multiple assertion" system, so if we re-threw them, they'd actually be reported twice
        }
        catch (Exception e)
        {
            if (e is TargetInvocationException ex)
            {
                e = ex.InnerException ?? e;
            }

            Assert.Fail(
                $"{Red}☠{FG} The method {methodString} caused a {Red}{e.GetType().Name}{FG} error: {e.Message}");
        }
    }

    private static string FormatLambda(
        ReadOnlySpan<char> expression,
        params object?[] args
    )
    {
        var arrowIndex = expression.IndexOf("=>");
        if (arrowIndex >= 0)
        {
            expression = expression[arrowIndex..];
        }

        var sb = new StringBuilder();

        sb.Append(MethodColor);
        sb.Append(expression);
        sb.Append(FG);
        sb.Append('(');

        for (int i = 0; i < args.Length; i++)
        {
            if (i > 0)
            {
                sb.Append(FG);
                sb.Append(", ");
            }

            sb.Append(ArgColor);
            sb.Append(args[i]);
        }

        sb.Append(FG);
        sb.Append(')');

        return sb.ToString();
    }

    private static object?[] ToArray(this ITuple tuple)
    {
        var objs = new object?[tuple.Length];
        for (int i = 0; i < tuple.Length; i++)
        {
            objs[i] = tuple[i];
        }

        return objs;
    }
}