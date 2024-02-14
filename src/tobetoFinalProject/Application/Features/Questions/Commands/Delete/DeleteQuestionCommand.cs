using Application.Features.Questions.Constants;
using Application.Features.Questions.Constants;
using Application.Features.Questions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Questions.Constants.QuestionsOperationClaims;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Questions.Commands.Delete;

public class DeleteQuestionCommand : IRequest<DeletedQuestionResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => new[] { Admin, Write, QuestionsOperationClaims.Delete };

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, DeletedQuestionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;
        private readonly QuestionBusinessRules _questionBusinessRules;

        public DeleteQuestionCommandHandler(IMapper mapper, IQuestionRepository questionRepository,
                                         QuestionBusinessRules questionBusinessRules)
        {
            _mapper = mapper;
            _questionRepository = questionRepository;
            _questionBusinessRules = questionBusinessRules;
        }

        public async Task<DeletedQuestionResponse> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            Question? question = await _questionRepository.GetAsync(predicate: q => q.Id == request.Id,
                include:q=>q.Include(q=>q.QuestionOptions)
                            .Include(q=>q.QuizQuestion), 
                cancellationToken: cancellationToken);
            await _questionBusinessRules.QuestionShouldExistWhenSelected(question);

            await _questionRepository.DeleteAsync(question!);

            DeletedQuestionResponse response = _mapper.Map<DeletedQuestionResponse>(question);
            return response;
        }
    }
}