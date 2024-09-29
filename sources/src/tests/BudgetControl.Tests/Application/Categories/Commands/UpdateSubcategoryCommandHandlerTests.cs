using System;
using BudgetControl.Application.Categories.Commands;
using BudgetControl.Common.Primitives.Persistence;
using BudgetControl.Common.Primitives.Results;
using BudgetControl.Domain.Categories;
using BudgetControl.Tests.Builders;
using Microsoft.Extensions.Logging;

namespace BudgetControl.Tests.Application.Categories.Commands;

[Collection("CategoryCommandHandlers"), Trait(nameof(UpdateSubcategoryCommandHandler), "Unit"), ExcludeFromCodeCoverage]
public class UpdateSubcategoryCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly ILogger<UpdateSubcategoryCommandHandler> _logger = Substitute.For<ILogger<UpdateSubcategoryCommandHandler>>();
    private UpdateSubcategoryCommandHandler _subject = null!;

    public UpdateSubcategoryCommandHandlerTests()
    {
        _fixture.Customize<Category>(c => c.FromFactory(() => new CategoryBuilder()
            .WithTitle(_fixture.Create<string>())
            .WithDescription(_fixture.Create<string>())
            .WithType("Credit")
            .WithSubcategories(new SubcategoryBuilder()
                .WithTitle(_fixture.Create<string>())
                .WithDescription(_fixture.Create<string>())
                .Build())
            .Build()));
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var category = _fixture.Create<Category>();
        var command = new UpdateSubcategoryCommand(_fixture.Create<string>(), _fixture.Create<string>());
        command.SetCategoryId(category.Id.Value);
        command.SetId(category.Subcategories.First().Id.Value);
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(category));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new UpdateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        _categoryRepository.Received(1).UpdateSubcategory(Arg.Any<Subcategory>());
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Description")]
    [InlineData("Title", "")]
    public async Task Handle_InvalidCommand_ReturnsFailure(string title, string description)
    {
        // Arrange
        var category = _fixture.Create<Category>();
        var command = new UpdateSubcategoryCommand(title, description);
        command.SetCategoryId(category.Id.Value);
        command.SetId(category.Subcategories.First().Id.Value);
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(category));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new UpdateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        _categoryRepository.DidNotReceive().UpdateSubcategory(Arg.Any<Subcategory>());
        await _unitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_InvalidCategory_ReturnsFailure()
    {
        // Arrange
        var command = new UpdateSubcategoryCommand(_fixture.Create<string>(), _fixture.Create<string>());
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult((Category?)null));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new UpdateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        _categoryRepository.DidNotReceive().UpdateSubcategory(Arg.Any<Subcategory>());
        await _unitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_InvalidSubcategory_ReturnsFailure()
    {
        // Arrange
        var category = _fixture.Create<Category>();
        var command = new UpdateSubcategoryCommand(_fixture.Create<string>(), _fixture.Create<string>());
        command.SetCategoryId(category.Id.Value);
        command.SetId(Guid.NewGuid());
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(category));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Success(true));

        _subject = new UpdateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        _categoryRepository.DidNotReceive().UpdateSubcategory(Arg.Any<Subcategory>());
        await _unitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_FailedCommit_ReturnsFailure()
    {
        // Arrange
        var category = _fixture.Create<Category>();
        var command = new UpdateSubcategoryCommand(_fixture.Create<string>(), _fixture.Create<string>());
        command.SetCategoryId(category.Id.Value);
        command.SetId(category.Subcategories.First().Id.Value);
        _categoryRepository.GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None)!.Returns(Task.FromResult(category));
        _unitOfWork.CommitAsync(CancellationToken.None).Returns(Result.Failures(_fixture.CreateMany<Error>()));

        _subject = new UpdateSubcategoryCommandHandler(_unitOfWork, _categoryRepository, _logger);

        // Act
        var result = await _subject.Handle(command, CancellationToken.None);

        // Assert
        _ = new AssertionScope();
        await _categoryRepository.Received(1).GetByIdWithReferencesAsync(Arg.Any<CategoryId>(), CancellationToken.None);
        _categoryRepository.Received(1).UpdateSubcategory(Arg.Any<Subcategory>());
        await _unitOfWork.Received(1).CommitAsync(CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
    }
}
