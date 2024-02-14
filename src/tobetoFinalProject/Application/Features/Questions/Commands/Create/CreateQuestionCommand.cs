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
using Application.Services.ImageService;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Questions.Commands.Create;

public class CreateQuestionCommand : IRequest<CreatedQuestionResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public IFormFile? ImageUrl { get; set; }
    public string Sentence { get; set; }
    public int CorrectOptionId { get; set; }
    public ICollection<QuestionOption> QuestionOptions { get; set; }
    public string[] Roles => new[] { Admin, Write, QuestionsOperationClaims.Create };

    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, CreatedQuestionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;
        private readonly QuestionBusinessRules _questionBusinessRules;
        private readonly ImageServiceBase _�mageServiceBase;


        public CreateQuestionCommandHandler(IMapper mapper, IQuestionRepository questionRepository,
                                         QuestionBusinessRules questionBusinessRules, ImageServiceBase �mageServiceBase)
        {
            _mapper = mapper;
            _questionRepository = questionRepository;
            _questionBusinessRules = questionBusinessRules;
            _�mageServiceBase = �mageServiceBase;
        }

        public async Task<CreatedQuestionResponse> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            Question question = _mapper.Map<Question>(request);
            await _questionBusinessRules.QuestionOptionsCountMustBeLessThanSevenWhenInsert(question.QuestionOptions.Count);
            await _questionBusinessRules.QuestionOptionsMustBeDifferent(question.QuestionOptions);
            if (request.ImageUrl is not null)
                question.ImageUrl = await _�mageServiceBase.UploadAsync(request.ImageUrl);

            await _questionRepository.AddAsync(question);

            CreatedQuestionResponse response = _mapper.Map<CreatedQuestionResponse>(question);
            return response;
        }
    }
}