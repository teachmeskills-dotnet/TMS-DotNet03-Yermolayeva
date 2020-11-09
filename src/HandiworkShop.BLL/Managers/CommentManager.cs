using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Managers
{
    public class CommentManager : ICommentManager
    {
        private readonly IRepository<Comment> _repositoryComment;

        public CommentManager(IRepository<Comment> repositoryComment)
        {
            _repositoryComment = repositoryComment ?? throw new ArgumentNullException(nameof(repositoryComment));
        }

        public async System.Threading.Tasks.Task CreateAsync(CommentDto commentDto)
        {
            var comment = new Comment
            {
                Rating = commentDto.Rating,
                Text = commentDto.Text,
                Created = DateTime.Now,
                AuthorId = commentDto.AuthorId,
                ProfileId = commentDto.ProfileId
            };

            await _repositoryComment.CreateAsync(comment);
            await _repositoryComment.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var comment = await _repositoryComment.GetEntityAsync(comment => comment.Id == id);
            if (comment is null)
            {
                return;
            }

            _repositoryComment.Delete(comment);
            await _repositoryComment.SaveChangesAsync();
        }

        public async Task<CommentDto> GetCommentAsync(int id)
        {
            var comment = await _repositoryComment.GetEntityAsync(comment => comment.Id == id);
            if (comment is null)
            {
                return null;
            }

            var commentDto = new CommentDto
            {
                Id = comment.Id,
                Rating = comment.Rating,
                Text = comment.Text,
                Created = comment.Created,
                AuthorId = comment.AuthorId,
                ProfileId = comment.ProfileId
            };
            return commentDto;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(string userId)
        {
            var commentDtos = new List<CommentDto>();
            var comments = await _repositoryComment
                .GetAll()
                .AsNoTracking()
                .Where(comment => comment.ProfileId == userId)
                .ToListAsync();

            if (!comments.Any())
            {
                return commentDtos;
            }

            foreach (var comment in comments)
            {
                commentDtos.Add(new CommentDto
                {
                    Id = comment.Id,
                    Rating = comment.Rating,
                    Text = comment.Text,
                    Created = comment.Created,
                    AuthorId = comment.AuthorId,
                    ProfileId = comment.ProfileId
                });
            }

            return commentDtos;
        }

        public async System.Threading.Tasks.Task UpdateCommentAsync(CommentDto commentDto)
        {
            var comment = await _repositoryComment.GetEntityAsync(comment => comment.Id == commentDto.Id);
            if (comment is null)
            {
                return;
            }

            static bool ValidateToUpdate(Comment comment, CommentDto commentDto)
            {
                bool updated = false;

                if (comment.Text != commentDto.Text)
                {
                    comment.Text = commentDto.Text;
                    updated = true;
                }

                if (comment.Rating != commentDto.Rating)
                {
                    comment.Rating = commentDto.Rating;
                    updated = true;
                }

                return updated;
            }

            var result = ValidateToUpdate(comment, commentDto);
            if (result)
            {
                await _repositoryComment.SaveChangesAsync();
            }
        }
    }
}
