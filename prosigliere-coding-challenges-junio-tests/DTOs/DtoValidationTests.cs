using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using prosigliere_coding_challenges_junio.DTOs;

namespace prosigliere_coding_challenges_junio_tests.DTOs
{
    public class DtoValidationTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        #region CreateBlogPostDto Tests

        [Fact]
        public void CreateBlogPostDto_WithValidData_PassesValidation()
        {
            // Arrange
            var dto = new CreateBlogPostDto
            {
                Title = "Valid Title",
                Content = "Valid content for the blog post"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CreateBlogPostDto_WithEmptyTitle_FailsValidation()
        {
            // Arrange
            var dto = new CreateBlogPostDto
            {
                Title = "",
                Content = "Valid content"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Title");
        }

        [Fact]
        public void CreateBlogPostDto_WithNullTitle_FailsValidation()
        {
            // Arrange
            var dto = new CreateBlogPostDto
            {
                Title = null!,
                Content = "Valid content"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Title");
        }

        [Fact]
        public void CreateBlogPostDto_WithTitleTooLong_FailsValidation()
        {
            // Arrange
            var dto = new CreateBlogPostDto
            {
                Title = new string('a', 201), // Exceeds 200 character limit
                Content = "Valid content"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Title");
        }

        [Fact]
        public void CreateBlogPostDto_WithEmptyContent_FailsValidation()
        {
            // Arrange
            var dto = new CreateBlogPostDto
            {
                Title = "Valid Title",
                Content = ""
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Content");
        }

        [Fact]
        public void CreateBlogPostDto_WithNullContent_FailsValidation()
        {
            // Arrange
            var dto = new CreateBlogPostDto
            {
                Title = "Valid Title",
                Content = null!
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Content");
        }

        [Fact]
        public void CreateBlogPostDto_WithContentTooLong_FailsValidation()
        {
            // Arrange
            var dto = new CreateBlogPostDto
            {
                Title = "Valid Title",
                Content = new string('a', 5001) // Exceeds 5000 character limit
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Content");
        }

        [Fact]
        public void CreateBlogPostDto_WithMaxLengthValues_PassesValidation()
        {
            // Arrange
            var dto = new CreateBlogPostDto
            {
                Title = new string('a', 200), // Exactly 200 characters
                Content = new string('b', 5000) // Exactly 5000 characters
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        #endregion

        #region CreateCommentDto Tests

        [Fact]
        public void CreateCommentDto_WithValidData_PassesValidation()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = "Valid comment content",
                Author = "Valid Author"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CreateCommentDto_WithEmptyContent_FailsValidation()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = "",
                Author = "Valid Author"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Content");
        }

        [Fact]
        public void CreateCommentDto_WithNullContent_FailsValidation()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = null!,
                Author = "Valid Author"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Content");
        }

        [Fact]
        public void CreateCommentDto_WithContentTooLong_FailsValidation()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = new string('a', 1001), // Exceeds 1000 character limit
                Author = "Valid Author"
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Content");
        }

        [Fact]
        public void CreateCommentDto_WithEmptyAuthor_FailsValidation()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = "Valid content",
                Author = ""
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Author");
        }

        [Fact]
        public void CreateCommentDto_WithNullAuthor_FailsValidation()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = "Valid content",
                Author = null!
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Author");
        }

        [Fact]
        public void CreateCommentDto_WithAuthorTooLong_FailsValidation()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = "Valid content",
                Author = new string('a', 101) // Exceeds 100 character limit
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Contain("Author");
        }

        [Fact]
        public void CreateCommentDto_WithMaxLengthValues_PassesValidation()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = new string('a', 1000), // Exactly 1000 characters
                Author = new string('b', 100) // Exactly 100 characters
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void CreateCommentDto_WithMultipleValidationErrors_ReturnsAllErrors()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Content = "", // Invalid - empty
                Author = "" // Invalid - empty
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            validationResults.Should().HaveCount(2);
            validationResults.Should().Contain(vr => vr.ErrorMessage!.Contains("Content"));
            validationResults.Should().Contain(vr => vr.ErrorMessage!.Contains("Author"));
        }

        #endregion
    }
}
