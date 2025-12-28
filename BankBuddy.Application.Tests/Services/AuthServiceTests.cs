using AutoMapper;
using BankBuddy.Application.DTOs.Auth;
using BankBuddy.Application.Interfaces.IRepositories;
using BankBuddy.Application.Interfaces.IServices;
using BankBuddy.Application.Services;
using BankBuddy.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IGenericRepository<User>> _userRepo;
        private readonly Mock<IGenericRepository<Role>> _roleRepo;
        private readonly Mock<IGenericRepository<RefreshToken>> _refreshTokenRepo;
        private readonly Mock<IGenericRepository<VerificationToken>> _verificationTokenRepo;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IConfiguration> _config;
        private readonly Mock<IMapper> _mapper;

        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            _userRepo = new Mock<IGenericRepository<User>>();
            _roleRepo = new Mock<IGenericRepository<Role>>();
            _refreshTokenRepo = new Mock<IGenericRepository<RefreshToken>>();
            _verificationTokenRepo = new Mock<IGenericRepository<VerificationToken>>();
            _emailService = new Mock<IEmailService>();
            _config = new Mock<IConfiguration>();
            _mapper = new Mock<IMapper>();

            _config.Setup(c => c["Jwt:Key"]).Returns("THIS_IS_A_SECRET_TEST_KEY_123456");
            _config.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _config.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
            _config.Setup(c => c["Jwt:ExpiresInMinutes"]).Returns("60");

            _authService = new AuthService(
                _userRepo.Object,
                _roleRepo.Object,
                _refreshTokenRepo.Object,
                _verificationTokenRepo.Object,
                _emailService.Object,
                _config.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_WhenEmailIsUnique_ShouldCreateUser()
        {
            // arrange
            RegisterDTO dto = new()
            {
                Email = "test@email.com",
                Password = "Password123"
            };

            Role role = new()
            {
                RoleId = Guid.NewGuid(),
                Name = "Customer"
            };

            User user = new()
            {
                UserId = Guid.NewGuid(),
                Email = dto.Email
            };

            _userRepo.Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>()
                ))
                .ReturnsAsync((User?)null);

            _roleRepo.Setup(r =>
                r.FindAsync(
                    It.IsAny<Expression<Func<Role, bool>>>(),
                    It.IsAny<Func<IQueryable<Role>, IQueryable<Role>>?>()
                ))
                .ReturnsAsync(role);

            _mapper.Setup(m => m.Map<User>(dto))
                   .Returns(user);

            _mapper.Setup(m => m.Map<AuthResponseDTO>(It.IsAny<AuthData>()))
                   .Returns(new AuthResponseDTO());

            // act
            AuthResponseDTO result = await _authService.RegisterAsync(dto);

            // assert
            result.Should().NotBeNull();

            _userRepo.Verify(r => r.AddAsync(It.Is<User>(
                u => u.Email == dto.Email && u.RoleId == role.RoleId
            )), Times.Once);
            _userRepo.Verify(r => r.SaveChangesAsync(), Times.Once);

            _refreshTokenRepo.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
            _refreshTokenRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
