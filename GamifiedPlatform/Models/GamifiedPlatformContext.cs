using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GamifiedPlatform.Models;

public partial class GamifiedPlatformContext : DbContext
{
    public GamifiedPlatformContext()
    {
    }

    public GamifiedPlatformContext(DbContextOptions<GamifiedPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Assessment> Assessments { get; set; }

    public virtual DbSet<Badge> Badges { get; set; }

    public virtual DbSet<Collaborative> Collaboratives { get; set; }

    public virtual DbSet<ContentLibrary> ContentLibraries { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseEnrollment> CourseEnrollments { get; set; }

    public virtual DbSet<DiscussionForum> DiscussionForums { get; set; }

    public virtual DbSet<EmotionalFeedback> EmotionalFeedbacks { get; set; }

    public virtual DbSet<EmotionalfeedbackReview> EmotionalfeedbackReviews { get; set; }

    public virtual DbSet<FilledSurvey> FilledSurveys { get; set; }

    public virtual DbSet<HealthCondition> HealthConditions { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<InteractionLog> InteractionLogs { get; set; }

    public virtual DbSet<Leaderboard> Leaderboards { get; set; }

    public virtual DbSet<Learner> Learners { get; set; }

    public virtual DbSet<LearnerDiscussion> LearnerDiscussions { get; set; }

    public virtual DbSet<LearnerMastery> LearnerMasteries { get; set; }

    public virtual DbSet<LearnersCollaboration> LearnersCollaborations { get; set; }

    public virtual DbSet<LearningActivity> LearningActivities { get; set; }

    public virtual DbSet<LearningGoal> LearningGoals { get; set; }

    public virtual DbSet<LearningPath> LearningPaths { get; set; }

    public virtual DbSet<LearningPreference> LearningPreferences { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<ModuleContent> ModuleContents { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Pathreview> Pathreviews { get; set; }

    public virtual DbSet<PersonalizationProfile> PersonalizationProfiles { get; set; }

    public virtual DbSet<Quest> Quests { get; set; }

    public virtual DbSet<QuestReward> QuestRewards { get; set; }

    public virtual DbSet<Ranking> Rankings { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<SkillMastery> SkillMasteries { get; set; }

    public virtual DbSet<SkillProgression> SkillProgressions { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }

    public virtual DbSet<Takesassesment> Takesassesments { get; set; }

    public virtual DbSet<TargetTrait> TargetTraits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GamifiedPlatform;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.AchievementId).HasName("PK__Achievem__276330E067DBB8C5");

            entity.ToTable("Achievement");

            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.BadgeId).HasColumnName("BadgeID");
            entity.Property(e => e.DateEarned)
                .HasColumnType("datetime")
                .HasColumnName("date_earned");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");

            entity.HasOne(d => d.Badge).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.BadgeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Achieveme__Badge__160F4887");

            entity.HasOne(d => d.Learner).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Achieveme__Learn__151B244E");
        });

        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Assessme__3214EC275EE3B557");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Criteria)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("criteria");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.PassingMarks).HasColumnName("passing_marks");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.TotalMarks).HasColumnName("total_marks");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.Weightage).HasColumnName("weightage");

            entity.HasOne(d => d.Module).WithMany(p => p.Assessments)
                .HasForeignKey(d => new { d.ModuleId, d.CourseId })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Assessments__5535A963");
        });

        modelBuilder.Entity<Badge>(entity =>
        {
            entity.HasKey(e => e.BadgeId).HasName("PK__Badge__1918237CC9CCDA01");

            entity.ToTable("Badge");

            entity.Property(e => e.BadgeId).HasColumnName("BadgeID");
            entity.Property(e => e.Criteria)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("criteria");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Collaborative>(entity =>
        {
            entity.HasKey(e => e.QuestId).HasName("PK__Collabor__B6619ACBBBF0E0FC");

            entity.ToTable("Collaborative");

            entity.Property(e => e.QuestId)
                .ValueGeneratedNever()
                .HasColumnName("QuestID");
            entity.Property(e => e.Deadline)
                .HasColumnType("datetime")
                .HasColumnName("deadline");
            entity.Property(e => e.MaxNumParticipants).HasColumnName("max_num_participants");

            entity.HasOne(d => d.Quest).WithOne(p => p.Collaborative)
                .HasForeignKey<Collaborative>(d => d.QuestId)
                .HasConstraintName("FK__Collabora__Quest__1F98B2C1");
        });

        modelBuilder.Entity<ContentLibrary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContentL__3214EC27DBF13B21");

            entity.ToTable("ContentLibrary");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ContentUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("content_URL");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Metadata)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("metadata");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");

            entity.HasOne(d => d.Module).WithMany(p => p.ContentLibraries)
                .HasForeignKey(d => new { d.ModuleId, d.CourseId })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ContentLibrary__52593CB8");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71871B81A91D");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CreditPoints).HasColumnName("credit_points");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.DifficultyLevel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("difficulty_level");
            entity.Property(e => e.LearningObjective)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("learning_objective");
            entity.Property(e => e.PreRequisites)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pre_requisites");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Courses).WithMany(p => p.Prereqs)
                .UsingEntity<Dictionary<string, object>>(
                    "Prerequisite",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Prerequis__cours__45F365D3"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("Prereq")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Prerequis__prere__46E78A0C"),
                    j =>
                    {
                        j.HasKey("CourseId", "Prereq").HasName("PK__Prerequi__1141CCC73C01BF0B");
                        j.ToTable("Prerequisites");
                        j.IndexerProperty<int>("CourseId").HasColumnName("course_id");
                        j.IndexerProperty<int>("Prereq").HasColumnName("prereq");
                    });

            entity.HasMany(d => d.Prereqs).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "Prerequisite",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("Prereq")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Prerequis__prere__46E78A0C"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Prerequis__cours__45F365D3"),
                    j =>
                    {
                        j.HasKey("CourseId", "Prereq").HasName("PK__Prerequi__1141CCC73C01BF0B");
                        j.ToTable("Prerequisites");
                        j.IndexerProperty<int>("CourseId").HasColumnName("course_id");
                        j.IndexerProperty<int>("Prereq").HasColumnName("prereq");
                    });
        });

        modelBuilder.Entity<CourseEnrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Course_e__7F6877FBF17DAA6E");

            entity.ToTable("Course_enrollment");

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.CompletionDate)
                .HasColumnType("datetime")
                .HasColumnName("completion_date");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.EnrollmentDate)
                .HasColumnType("datetime")
                .HasColumnName("enrollment_date");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseEnrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Course_en__Cours__6E01572D");

            entity.HasOne(d => d.Learner).WithMany(p => p.CourseEnrollments)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Course_en__Learn__6EF57B66");
        });

        modelBuilder.Entity<DiscussionForum>(entity =>
        {
            entity.HasKey(e => e.ForumId).HasName("PK__Discussi__BBA7A4405D8D6BFA");

            entity.ToTable("Discussion_forum");

            entity.Property(e => e.ForumId).HasColumnName("forumID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.LastActive)
                .HasColumnType("datetime")
                .HasColumnName("last_active");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Module).WithMany(p => p.DiscussionForums)
                .HasForeignKey(d => new { d.ModuleId, d.CourseId })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Discussion_forum__2A164134");
        });

        modelBuilder.Entity<EmotionalFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Emotiona__6A4BEDF6EEEA597A");

            entity.ToTable("Emotional_feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.EmotionalState)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("emotional_state");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Learner).WithMany(p => p.EmotionalFeedbacks)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Emotional__Learn__5EBF139D");
        });

        modelBuilder.Entity<EmotionalfeedbackReview>(entity =>
        {
            entity.HasKey(e => new { e.FeedbackId, e.InstructorId }).HasName("PK__Emotiona__C39BFD413357ED48");

            entity.ToTable("Emotionalfeedback_review");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.Feedback)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("feedback");

            entity.HasOne(d => d.FeedbackNavigation).WithMany(p => p.EmotionalfeedbackReviews)
                .HasForeignKey(d => d.FeedbackId)
                .HasConstraintName("FK__Emotional__Feedb__6A30C649");

            entity.HasOne(d => d.Instructor).WithMany(p => p.EmotionalfeedbackReviews)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Emotional__Instr__6B24EA82");
        });

        modelBuilder.Entity<FilledSurvey>(entity =>
        {
            entity.HasKey(e => new { e.SurveyId, e.Question, e.LearnerId }).HasName("PK__FilledSu__D89C33C7690DCDD4");

            entity.ToTable("FilledSurvey");

            entity.Property(e => e.SurveyId).HasColumnName("SurveyID");
            entity.Property(e => e.Question)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Answer)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Learner).WithMany(p => p.FilledSurveys)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__FilledSur__Learn__07C12930");

            entity.HasOne(d => d.SurveyQuestion).WithMany(p => p.FilledSurveys)
                .HasForeignKey(d => new { d.SurveyId, d.Question })
                .HasConstraintName("FK__FilledSurvey__06CD04F7");
        });

        modelBuilder.Entity<HealthCondition>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.ProfileId, e.Condition }).HasName("PK__HealthCo__930320B074CC908A");

            entity.ToTable("HealthCondition");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.Condition)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("condition");

            entity.HasOne(d => d.PersonalizationProfile).WithMany(p => p.HealthConditions)
                .HasForeignKey(d => new { d.LearnerId, d.ProfileId })
                .HasConstraintName("FK__HealthCondition__412EB0B6");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InstructorId).HasName("PK__Instruct__9D010B7BDED0E7AE");

            entity.ToTable("Instructor");

            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.ExpertiseArea)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("expertise_area");
            entity.Property(e => e.LatestQualification)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("latest_qualification");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasMany(d => d.Courses).WithMany(p => p.Instructors)
                .UsingEntity<Dictionary<string, object>>(
                    "Teach",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .HasConstraintName("FK__Teaches__CourseI__72C60C4A"),
                    l => l.HasOne<Instructor>().WithMany()
                        .HasForeignKey("InstructorId")
                        .HasConstraintName("FK__Teaches__Instruc__71D1E811"),
                    j =>
                    {
                        j.HasKey("InstructorId", "CourseId").HasName("PK__Teaches__F193DC63802C6EFD");
                        j.ToTable("Teaches");
                        j.IndexerProperty<int>("InstructorId").HasColumnName("InstructorID");
                        j.IndexerProperty<int>("CourseId").HasColumnName("CourseID");
                    });
        });

        modelBuilder.Entity<InteractionLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Interact__5E5499A86F137EB1");

            entity.ToTable("Interaction_log");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ActionType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("action_type");
            entity.Property(e => e.ActivityId).HasColumnName("activity_ID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Activity).WithMany(p => p.InteractionLogs)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Interacti__activ__5AEE82B9");

            entity.HasOne(d => d.Learner).WithMany(p => p.InteractionLogs)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Interacti__Learn__5BE2A6F2");
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.BoardId).HasName("PK__Leaderbo__F9646BD255B25868");

            entity.ToTable("Leaderboard");

            entity.Property(e => e.BoardId).HasColumnName("BoardID");
            entity.Property(e => e.Season)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("season");
        });

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.HasKey(e => e.LearnerId).HasName("PK__Learner__67ABFCFA7C06D3AB");

            entity.ToTable("Learner");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.CulturalBackground)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cultural_background");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<LearnerDiscussion>(entity =>
        {
            entity.HasKey(e => new { e.ForumId, e.LearnerId }).HasName("PK__LearnerD__546A1380A1D6A2CA");

            entity.ToTable("LearnerDiscussion");

            entity.Property(e => e.ForumId).HasColumnName("ForumID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Post)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Time)
                .HasColumnType("datetime")
                .HasColumnName("time");

            entity.HasOne(d => d.Forum).WithMany(p => p.LearnerDiscussions)
                .HasForeignKey(d => d.ForumId)
                .HasConstraintName("FK__LearnerDi__Forum__2CF2ADDF");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearnerDiscussions)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__LearnerDi__Learn__2DE6D218");
        });

        modelBuilder.Entity<LearnerMastery>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.QuestId, e.Skill }).HasName("PK__LearnerM__36F2E773AE4474BA");

            entity.ToTable("LearnerMastery");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.QuestId).HasColumnName("QuestID");
            entity.Property(e => e.Skill)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("skill");
            entity.Property(e => e.CompletionStatus).HasColumnName("completion_status");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearnerMasteries)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearnerMa__Learn__2645B050");

            entity.HasOne(d => d.SkillMastery).WithMany(p => p.LearnerMasteries)
                .HasForeignKey(d => new { d.QuestId, d.Skill })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearnerMastery__2739D489");
        });

        modelBuilder.Entity<LearnersCollaboration>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.QuestId }).HasName("PK__Learners__CCCDE556D8051D62");

            entity.ToTable("LearnersCollaboration");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.QuestId).HasColumnName("QuestID");
            entity.Property(e => e.CompletionStatus).HasColumnName("completion_status");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearnersCollaborations)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearnersC__Learn__22751F6C");

            entity.HasOne(d => d.Quest).WithMany(p => p.LearnersCollaborations)
                .HasForeignKey(d => d.QuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearnersC__Quest__236943A5");
        });

        modelBuilder.Entity<LearningActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Learning__45F4A7F132572FAE");

            entity.ToTable("Learning_activities");

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ActivityType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("activity_type");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.InstructionDetails)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("instruction_details");
            entity.Property(e => e.MaxPoints).HasColumnName("Max_points");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

            entity.HasOne(d => d.Module).WithMany(p => p.LearningActivities)
                .HasForeignKey(d => new { d.ModuleId, d.CourseId })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Learning_activit__5812160E");
        });

        modelBuilder.Entity<LearningGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Learning__3214EC27E361E771");

            entity.ToTable("Learning_goal");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Deadline)
                .HasColumnType("datetime")
                .HasColumnName("deadline");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasMany(d => d.Learners).WithMany(p => p.Goals)
                .UsingEntity<Dictionary<string, object>>(
                    "LearnersGoal",
                    r => r.HasOne<Learner>().WithMany()
                        .HasForeignKey("LearnerId")
                        .HasConstraintName("FK__LearnersG__Learn__7F2BE32F"),
                    l => l.HasOne<LearningGoal>().WithMany()
                        .HasForeignKey("GoalId")
                        .HasConstraintName("FK__LearnersG__GoalI__7E37BEF6"),
                    j =>
                    {
                        j.HasKey("GoalId", "LearnerId").HasName("PK__Learners__3C3540FEE4FDD327");
                        j.ToTable("LearnersGoals");
                        j.IndexerProperty<int>("GoalId").HasColumnName("GoalID");
                        j.IndexerProperty<int>("LearnerId").HasColumnName("LearnerID");
                    });
        });

        modelBuilder.Entity<LearningPath>(entity =>
        {
            entity.HasKey(e => e.PathId).HasName("PK__Learning__BFB8200AAF981396");

            entity.ToTable("Learning_path");

            entity.Property(e => e.PathId).HasColumnName("pathID");
            entity.Property(e => e.AdaptiveRules)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("adaptive_rules");
            entity.Property(e => e.CompletionStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("completion_status");
            entity.Property(e => e.CustomContent)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("custom_content");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");

            entity.HasOne(d => d.PersonalizationProfile).WithMany(p => p.LearningPaths)
                .HasForeignKey(d => new { d.LearnerId, d.ProfileId })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Learning_path__619B8048");
        });

        modelBuilder.Entity<LearningPreference>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.Preference }).HasName("PK__Learning__6032E158B843FE05");

            entity.ToTable("LearningPreference");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Preference)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("preference");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearningPreferences)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__LearningP__Learn__3B75D760");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => new { e.ModuleId, e.CourseId }).HasName("PK__Modules__47E6A09F2A6F4D3A");

            entity.Property(e => e.ModuleId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ModuleID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.ContentUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contentURL");
            entity.Property(e => e.Difficulty)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("difficulty");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Course).WithMany(p => p.Modules)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Modules__CourseI__49C3F6B7");
        });

        modelBuilder.Entity<ModuleContent>(entity =>
        {
            entity.HasKey(e => new { e.ModuleId, e.CourseId, e.ContentType }).HasName("PK__ModuleCo__402E75DA1ECFE34B");

            entity.ToTable("ModuleContent");

            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.ContentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("content_type");

            entity.HasOne(d => d.Module).WithMany(p => p.ModuleContents)
                .HasForeignKey(d => new { d.ModuleId, d.CourseId })
                .HasConstraintName("FK__ModuleContent__4F7CD00D");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC27CCF2330B");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Message)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UrgencyLevel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("urgency_level");

            entity.HasMany(d => d.Learners).WithMany(p => p.Notifications)
                .UsingEntity<Dictionary<string, object>>(
                    "ReceivedNotification",
                    r => r.HasOne<Learner>().WithMany()
                        .HasForeignKey("LearnerId")
                        .HasConstraintName("FK__ReceivedN__Learn__0D7A0286"),
                    l => l.HasOne<Notification>().WithMany()
                        .HasForeignKey("NotificationId")
                        .HasConstraintName("FK__ReceivedN__Notif__0C85DE4D"),
                    j =>
                    {
                        j.HasKey("NotificationId", "LearnerId").HasName("PK__Received__96B591FD3CD073AD");
                        j.ToTable("ReceivedNotification");
                        j.IndexerProperty<int>("NotificationId").HasColumnName("NotificationID");
                        j.IndexerProperty<int>("LearnerId").HasColumnName("LearnerID");
                    });
        });

        modelBuilder.Entity<Pathreview>(entity =>
        {
            entity.HasKey(e => new { e.InstructorId, e.PathId }).HasName("PK__Pathrevi__11D776B8558156A5");

            entity.ToTable("Pathreview");

            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.PathId).HasColumnName("PathID");
            entity.Property(e => e.Feedback)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("feedback");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Pathreviews)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Pathrevie__Instr__66603565");

            entity.HasOne(d => d.Path).WithMany(p => p.Pathreviews)
                .HasForeignKey(d => d.PathId)
                .HasConstraintName("FK__Pathrevie__PathI__6754599E");
        });

        modelBuilder.Entity<PersonalizationProfile>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.ProfileId }).HasName("PK__Personal__353B34726CC20F7D");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.EmotionalState)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("emotional_state");
            entity.Property(e => e.PersonalityType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("personality_type");
            entity.Property(e => e.PreferedContentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Prefered_content_type");

            entity.HasOne(d => d.Learner).WithMany(p => p.PersonalizationProfiles)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__Personali__Learn__3E52440B");
        });

        modelBuilder.Entity<Quest>(entity =>
        {
            entity.HasKey(e => e.QuestId).HasName("PK__Quest__B6619ACB13F0B73C");

            entity.ToTable("Quest");

            entity.Property(e => e.QuestId).HasColumnName("QuestID");
            entity.Property(e => e.Criteria)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("criteria");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.DifficultyLevel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("difficulty_level");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<QuestReward>(entity =>
        {
            entity.HasKey(e => new { e.RewardId, e.QuestId, e.LearnerId }).HasName("PK__QuestRew__D251A7C9F611D0DF");

            entity.ToTable("QuestReward");

            entity.Property(e => e.RewardId).HasColumnName("RewardID");
            entity.Property(e => e.QuestId).HasColumnName("QuestID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.TimeEarned)
                .HasColumnType("datetime")
                .HasColumnName("Time_earned");

            entity.HasOne(d => d.Learner).WithMany(p => p.QuestRewards)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__QuestRewa__Learn__32AB8735");

            entity.HasOne(d => d.Quest).WithMany(p => p.QuestRewards)
                .HasForeignKey(d => d.QuestId)
                .HasConstraintName("FK__QuestRewa__Quest__31B762FC");

            entity.HasOne(d => d.Reward).WithMany(p => p.QuestRewards)
                .HasForeignKey(d => d.RewardId)
                .HasConstraintName("FK__QuestRewa__Rewar__30C33EC3");
        });

        modelBuilder.Entity<Ranking>(entity =>
        {
            entity.HasKey(e => new { e.BoardId, e.LearnerId, e.CourseId }).HasName("PK__Ranking__C9D7F96C0FF9940F");

            entity.ToTable("Ranking");

            entity.Property(e => e.BoardId).HasColumnName("BoardID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Rank).HasColumnName("rank");
            entity.Property(e => e.TotalPoints).HasColumnName("total_points");

            entity.HasOne(d => d.Board).WithMany(p => p.Rankings)
                .HasForeignKey(d => d.BoardId)
                .HasConstraintName("FK__Ranking__BoardID__778AC167");

            entity.HasOne(d => d.Course).WithMany(p => p.Rankings)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Ranking__CourseI__797309D9");

            entity.HasOne(d => d.Learner).WithMany(p => p.Rankings)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__Ranking__Learner__787EE5A0");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__Reward__825015993D18CE5F");

            entity.ToTable("Reward");

            entity.Property(e => e.RewardId).HasColumnName("RewardID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.Value)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("value");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.Skill1 }).HasName("PK__Skills__C45BDEA5EFD5A703");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Skill1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("skill");

            entity.HasOne(d => d.Learner).WithMany(p => p.Skills)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__Skills__LearnerI__38996AB5");
        });

        modelBuilder.Entity<SkillMastery>(entity =>
        {
            entity.HasKey(e => new { e.QuestId, e.Skill }).HasName("PK__Skill_Ma__1591B894FFC7C97E");

            entity.ToTable("Skill_Mastery");

            entity.Property(e => e.QuestId).HasColumnName("QuestID");
            entity.Property(e => e.Skill)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("skill");

            entity.HasOne(d => d.Quest).WithMany(p => p.SkillMasteries)
                .HasForeignKey(d => d.QuestId)
                .HasConstraintName("FK__Skill_Mas__Quest__1CBC4616");
        });

        modelBuilder.Entity<SkillProgression>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SkillPro__3214EC27D4AD2E0B");

            entity.ToTable("SkillProgression");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.ProficiencyLevel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("proficiency_level");
            entity.Property(e => e.SkillName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("skill_name");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Skill).WithMany(p => p.SkillProgressions)
                .HasForeignKey(d => new { d.LearnerId, d.SkillName })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SkillProgression__123EB7A3");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Survey__3214EC270E612E80");

            entity.ToTable("Survey");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SurveyQuestion>(entity =>
        {
            entity.HasKey(e => new { e.SurveyId, e.Question }).HasName("PK__SurveyQu__23FB983B6C44CF00");

            entity.Property(e => e.SurveyId).HasColumnName("SurveyID");
            entity.Property(e => e.Question)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Survey).WithMany(p => p.SurveyQuestions)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("FK__SurveyQue__Surve__03F0984C");
        });

        modelBuilder.Entity<Takesassesment>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.AssesmentId }).HasName("PK__takesass__4D87F993BC84C5CB");

            entity.ToTable("takesassesment");

            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.AssesmentId).HasColumnName("assesment_id");

            entity.HasOne(d => d.Assesment).WithMany(p => p.Takesassesments)
                .HasForeignKey(d => d.AssesmentId)
                .HasConstraintName("FK__takesasse__asses__367C1819");

            entity.HasOne(d => d.Learner).WithMany(p => p.Takesassesments)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__takesasse__learn__3587F3E0");
        });

        modelBuilder.Entity<TargetTrait>(entity =>
        {
            entity.HasKey(e => new { e.ModuleId, e.CourseId, e.Trait }).HasName("PK__Target_t__4E005E4C9BF9365B");

            entity.ToTable("Target_traits");

            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Trait)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Module).WithMany(p => p.TargetTraits)
                .HasForeignKey(d => new { d.ModuleId, d.CourseId })
                .HasConstraintName("FK__Target_traits__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
