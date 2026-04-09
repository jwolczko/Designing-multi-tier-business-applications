using FluentAssertions;
using Fortuna.Infrastructure.Auth;
using Xunit;

namespace Fortuna.UnitTests.Security;

public sealed class Pbkdf2PasswordHasherTests
{
    [Fact]
    public void Hash_And_Verify_Should_Succeed_For_Valid_Password()
    {
        var sut = new Pbkdf2PasswordHasher();

        var hash = sut.Hash("Secret123!");

        sut.Verify("Secret123!", hash).Should().BeTrue();
        sut.Verify("WrongPassword", hash).Should().BeFalse();
    }
}
