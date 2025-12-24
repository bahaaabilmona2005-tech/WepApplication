using AutoMapper;
using WebApplication1.DTOs.Answer;
using WebApplication1.DTOs.Certificate;
using WebApplication1.DTOs.Course;
using WebApplication1.DTOs.Enrollment;
using WebApplication1.DTOs.Lesson;
using WebApplication1.DTOs.LessonCompletion;
using WebApplication1.DTOs.Question;
using WebApplication1.DTOs.Quiz;
using WebApplication1.DTOs.QuizAttempt;
using WebApplication1.DTOs.QuizResponse;
using WebApplication1.DTOs.User;
using WebApplication1.Repository.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // USER
        CreateMap<User, UserReadDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();

        // COURSE
        CreateMap<Course, CourseReadDto>();
        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();

        // LESSON
        CreateMap<Lesson, LessonReadDto>();
        CreateMap<LessonCreateDto, Lesson>();
        CreateMap<LessonUpdateDto, Lesson>();

        // ENROLLMENT
        CreateMap<Enrollment, EnrollmentReadDto>();
        CreateMap<EnrollmentCreateDto, Enrollment>();

        // LESSON COMPLETION
        CreateMap<LessonCompletion, LessonCompletionReadDto>();
        CreateMap<LessonCompletionCreateDto, LessonCompletion>();

        // QUIZ
        CreateMap<Quiz, QuizReadDto>();
        CreateMap<QuizCreateDto, Quiz>();
        CreateMap<QuizUpdateDto, Quiz>();

        // QUESTION
        CreateMap<Question, QuestionReadDto>();
        CreateMap<QuestionCreateDto, Question>();
        CreateMap<QuestionUpdateDto, Question>();

        // ANSWER
        CreateMap<Answer, AnswerReadDto>();
        CreateMap<AnswerCreateDto, Answer>();
        CreateMap<AnswerUpdateDto, Answer>();

        // QUIZ ATTEMPT
        CreateMap<QuizAttempt, QuizAttemptReadDto>();
        CreateMap<QuizAttemptCreateDto, QuizAttempt>();

        // QUIZ RESPONSE
        CreateMap<QuizResponse, QuizResponseReadDto>();
        CreateMap<QuizResponseCreateDto, QuizResponse>();

        // CERTIFICATE
        CreateMap<Certificate, CertificateReadDto>();
        CreateMap<CertificateCreateDto, Certificate>();
    }
}
