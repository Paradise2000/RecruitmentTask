using Microsoft.EntityFrameworkCore;
using RecruitmentTask.DTOs;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;
using RecruitmentTask.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTask.Tests
{
    public class TagServiceTests
    {
        [Fact]
        public async Task GetTagsFromExternalAPI_AddsNewTags()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<RecruitmentTaskDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetTagsFromExternalAPI")
                .Options;

            using (var context = new RecruitmentTaskDbContext(dbContextOptions))
            {
                var tagService = new TagService(context);

                // Act
                await tagService.GetTagsFromExternalAPI(130);

                // Assert
                Assert.Equal(130, context.Tags.Count());
            }
        }

        [Fact]
        public async Task GetTags_ReturnPaginatedList()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<RecruitmentTaskDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetTags")
                .Options;

            using (var context = new RecruitmentTaskDbContext(dbContextOptions))
            {
                var tagService = new TagService(context);
                int pageIndex = 1;
                int pageSize = 10;
                string orderBy = "name";
                SortDirection sortDirection = SortDirection.DESC;

                // Act
                var result = await tagService.GetTags(pageIndex, pageSize, orderBy, sortDirection);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<PaginationModel<TagDto>>(result);
                Assert.Equal(pageIndex, result.PageIndex);
            }
        }

        [Fact]
        public async Task GetTags_ReturnPaginatedListSortedByName()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<RecruitmentTaskDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetTagsSortedByName")
                .Options;

            using (var context = new RecruitmentTaskDbContext(dbContextOptions))
            {
                context.Tags.AddRange(
                    new List<Tag>
                    {
                        new Tag { name = "python", count = 2192279, has_synonyms = true, is_moderator_only = false, is_required = false, percentageShare = 22.559261204130497 },
                        new Tag { name = "java", count = 1917266, has_synonyms = true, is_moderator_only = false, is_required = false, percentageShare = 19.72928833045359 },
                        new Tag { name = "javascript", count = 2528830, has_synonyms = true, is_moderator_only = false, is_required = false, percentageShare = 26.022480035999667 },
                        new Tag { name = "c#", count = 1615039, has_synonyms = true, is_moderator_only = false, is_required = false, percentageShare = 16.619274579493627 },
                        new Tag { name = "php", count = 1464453, has_synonyms = true, is_moderator_only = false, is_required = false, percentageShare = 15.06969584992262 }
                    }
                );
                context.SaveChanges();

                var tagService = new TagService(context);
                int pageIndex = 1;
                int pageSize = 5;
                string orderBy = "name";
                SortDirection sortDirection = SortDirection.ASC;

                // Act
                var result = await tagService.GetTags(pageIndex, pageSize, orderBy, sortDirection);

                // Assert
                Assert.Equal("c#", result.Items[0].name);
                Assert.Equal("java", result.Items[1].name);
                Assert.Equal("javascript", result.Items[2].name);
                Assert.Equal("php", result.Items[3].name);
                Assert.Equal("python", result.Items[4].name);
            }
        }

    }
}
