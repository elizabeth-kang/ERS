using Xunit;
using Moq;
using Services;
using CustomExceptions;
using DataAccess;
using Models;
using WebAPI.Controller;

namespace Tests;

public class AuthServiceTesting
{
    [Fact]
    public void LoginFailPasswordIncorrect()
    {
        var mockedRepo = new Mock<IUserDAO>();
        User correct = new User("Hydrangea", "Rose", Role.Employee);
        User incorrect = new User(1, "Hydrangea", "Lily", Role.Employee);

        mockedRepo.Setup(repo => repo.GetUserByUsername(correct.username)).Returns(correct);
        
        AuthServices service = new(mockedRepo.Object);

        Assert.Throws<UsernameNotAvailableException>(() => service.Register(incorrect));
    }

    [Fact]
    public void LoginFailUsernameDoesNotExist()
    {
        var mockedRepo = new Mock<IUserDAO>();
        User correct = new User("Hydrangea", "Rose", Role.Employee);
        User incorrect = new User(1, "Hydrange", "Ros", Role.Employee);

        mockedRepo.Setup(repo => repo.GetUserByUsername(correct.username)).Returns(correct);
        mockedRepo.Setup(repo => repo.GetUserByUsername(incorrect.username)).Throws<UsernameNotAvailableException>();
        AuthServices service = new(mockedRepo.Object);
        mockedRepo.Verify(repo => repo.GetUserByUsername(incorrect.username), Times.Never);
        Assert.Throws<UsernameNotAvailableException>(() => service.Register(incorrect));
    }

    [Fact]
    public void RegisterFailUsernameAlreadyExist()
    {
        var mockedRepo = new Mock<IUserDAO>();
        User correct = new User("Hydrangea", "Rose", Role.Employee);
        User incorrect = new User(1, "Hydrangea", "Rose", Role.Employee);
        mockedRepo.Setup(repo => repo.GetUserByUsername(correct.username)).Returns(incorrect);
        AuthServices service =new(mockedRepo.Object);
        Assert.Throws<UsernameNotAvailableException>(() => service.Register(correct));
        mockedRepo.Verify(repo=>repo.GetUserByUsername(correct.username),Times.Once());
    }
}