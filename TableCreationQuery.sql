CREATE DATABASE GamifiedPlatform
--test2
USE GamifiedPlatform
drop table users

Create table Users(
    UserID INT primary Key Identity,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Password varchar(50),
    Type varchar(50)
);


CREATE TABLE Admin (
    AdminID INT PRIMARY KEY IDENTITY,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    gender CHAR(1),
    birth_date DATE,
    country VARCHAR(50),
    email VARCHAR(50)
)

 --1
CREATE TABLE Learner (
    LearnerID INT PRIMARY KEY identity,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    gender CHAR(1),
    birth_date DATE,
    country VARCHAR(50),
    cultural_background VARCHAR(50)
);

-- 2
CREATE TABLE Skills (
    LearnerID INT,
    skill VARCHAR(50),
    PRIMARY KEY (LearnerID, skill),
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);

-- 3
CREATE TABLE LearningPreference (
    LearnerID INT,
    preference VARCHAR(50),
    PRIMARY KEY (LearnerID, preference),
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);

--4
CREATE TABLE PersonalizationProfiles (
    LearnerID INT,
    ProfileID INT,
    Prefered_content_type VARCHAR(50),
    emotional_state VARCHAR(50),
    personality_type VARCHAR(50),
    PRIMARY KEY (LearnerID, ProfileID),
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);

--5
CREATE TABLE HealthCondition (
    LearnerID INT,
    ProfileID INT,
    condition VARCHAR(50),
    PRIMARY KEY (LearnerID, ProfileID, condition),
    FOREIGN KEY (LearnerID, ProfileID) REFERENCES PersonalizationProfiles(LearnerID, ProfileID)on delete cascade on update cascade
);

--6
CREATE TABLE Course (
    CourseID INT PRIMARY KEY identity,
    Title VARCHAR(50),
    learning_objective VARCHAR(50),
    credit_points INT,
    difficulty_level VARCHAR(50),
    pre_requisites VARCHAR(50),
    description VARCHAR(50)
);
--7
CREATE TABLE Prerequisites (
    course_id INT,
    prereq INT,
    PRIMARY KEY (course_id, prereq),
    FOREIGN KEY (course_id) REFERENCES Course(CourseID),
    FOREIGN KEY (prereq) REFERENCES Course(CourseID)
);


-- 8
CREATE TABLE Modules (
    ModuleID INT identity ,
    CourseID INT,
    Title VARCHAR(50),
    difficulty VARCHAR(50),
    contentURL VARCHAR(50),
    PRIMARY KEY(ModuleID,CourseID),
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID)on delete cascade on update cascade
);

-- 9
CREATE TABLE Target_traits (
    ModuleID INT,
    CourseID INT,
    Trait VARCHAR(50),
    PRIMARY KEY (ModuleID, CourseID, Trait),
    FOREIGN KEY (ModuleID, CourseID) REFERENCES Modules(ModuleID, CourseID)on delete cascade on update cascade
);

--10
CREATE TABLE ModuleContent (
    ModuleID INT,
    CourseID INT,
    content_type VARCHAR(50),
    PRIMARY KEY (ModuleID, CourseID, content_type),
    FOREIGN KEY (ModuleID, CourseID) REFERENCES Modules(ModuleID, CourseID)on delete cascade on update cascade
);

--11
CREATE TABLE ContentLibrary (
    ID INT PRIMARY KEY identity,
    ModuleID INT,
    CourseID INT,
    Title VARCHAR(50),
    description VARCHAR(50),
    metadata VARCHAR(50),
    type VARCHAR(50),
    content_URL VARCHAR(50),
    FOREIGN KEY (ModuleID, CourseID) REFERENCES Modules(ModuleID, CourseID)on delete cascade on update cascade
);

--12
CREATE TABLE Assessments (
    ID INT PRIMARY KEY identity,
    ModuleID INT,
    CourseID INT,
    type VARCHAR(50),
    total_marks INT,
    passing_marks INT,
    criteria VARCHAR(50),
    weightage INT,
    description VARCHAR(50),
    title VARCHAR(50),
    FOREIGN KEY (ModuleID, CourseID) REFERENCES Modules(ModuleID, CourseID)on delete cascade on update cascade
);


-- 13
CREATE TABLE Learning_activities (
    ActivityID INT PRIMARY KEY identity,
    ModuleID INT,
    CourseID INT,
    activity_type VARCHAR(50),
    instruction_details VARCHAR(100),
    Max_points INT,
    FOREIGN KEY (ModuleID, CourseID) REFERENCES Modules(ModuleID, CourseID)on delete cascade on update cascade
);
-- 14
CREATE TABLE Interaction_log (
    LogID INT PRIMARY KEY identity,
    activity_ID INT,
    LearnerID INT,
    Duration TIME,
    Timestamp DATETIME ,
    action_type VARCHAR(50),
    FOREIGN KEY (activity_ID) REFERENCES Learning_activities(ActivityID)on delete cascade on update cascade,
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);


-- 15
CREATE TABLE Emotional_feedback (
    FeedbackID INT PRIMARY KEY identity,
    LearnerID INT,
    timestamp DATETIME,
    emotional_state VARCHAR(50),
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);

--16
CREATE TABLE Learning_path (
    pathID INT PRIMARY KEY identity,
    LearnerID INT,
    ProfileID INT,
    completion_status VARCHAR(50),
    custom_content VARCHAR(50),
    adaptive_rules VARCHAR(50),
    FOREIGN KEY (LearnerID, ProfileID) REFERENCES PersonalizationProfiles(LearnerID, ProfileID)on delete cascade on update cascade
);

-- 17
CREATE TABLE Instructor (
    InstructorID INT PRIMARY KEY identity,
    name VARCHAR(50),
    latest_qualification VARCHAR(50),
    expertise_area VARCHAR(50),
    email VARCHAR(50)
);

-- 18
CREATE TABLE Pathreview (
    InstructorID INT,
    PathID INT,
    feedback VARCHAR(50),
    PRIMARY KEY (InstructorID, PathID),
    FOREIGN KEY (InstructorID) REFERENCES Instructor(InstructorID)on delete cascade on update cascade,
    FOREIGN KEY (PathID) REFERENCES Learning_path(pathID)on delete cascade on update cascade
);

-- 19
CREATE TABLE Emotionalfeedback_review (
    FeedbackID INT,
    InstructorID INT,
    feedback VARCHAR(100),
    PRIMARY KEY (FeedbackID, InstructorID),
    FOREIGN KEY (FeedbackID) REFERENCES Emotional_feedback(FeedbackID)on delete cascade on update cascade,
    FOREIGN KEY (InstructorID) REFERENCES Instructor(InstructorID)on delete cascade on update cascade
);

--20
CREATE TABLE Course_enrollment (
    EnrollmentID INT PRIMARY KEY identity,
    CourseID INT,
    LearnerID INT,
    completion_date DATETIME,
    enrollment_date DATETIME,
    status VARCHAR(50),
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID)on delete cascade on update cascade,
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);

-- 21
CREATE TABLE Teaches (
    InstructorID INT,
    CourseID INT,
    PRIMARY KEY (InstructorID, CourseID),
    FOREIGN KEY (InstructorID) REFERENCES Instructor(InstructorID)on delete cascade on update cascade,
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID)on delete cascade on update cascade
);

-- 22
CREATE TABLE Leaderboard (
    BoardID INT PRIMARY KEY identity,
    season VARCHAR(50)
);

--23
CREATE TABLE Ranking (
    BoardID INT,
    LearnerID INT,
    CourseID INT,
    rank INT,
    total_points INT,
    PRIMARY KEY (BoardID, LearnerID, CourseID),
    FOREIGN KEY (BoardID) REFERENCES Leaderboard(BoardID)on delete cascade on update cascade,
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade,
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID)on delete cascade on update cascade
);

-- 24
CREATE TABLE Learning_goal (
    ID INT PRIMARY KEY identity,
    status VARCHAR(50),
    deadline DATETIME,
    description VARCHAR(50)
);

-- 25
CREATE TABLE LearnersGoals (
    GoalID INT,
    LearnerID INT,
    PRIMARY KEY (GoalID, LearnerID),
    FOREIGN KEY (GoalID) REFERENCES Learning_goal(ID)on delete cascade on update cascade,
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);

--26
CREATE TABLE Survey (
    ID INT PRIMARY KEY identity,
    Title VARCHAR(50)
);

-- 27
CREATE TABLE SurveyQuestions (
    SurveyID INT,
    Question VARCHAR(100),
    PRIMARY KEY (SurveyID, Question),
    FOREIGN KEY (SurveyID) REFERENCES Survey(ID)on delete cascade on update cascade
);

-- 28
CREATE TABLE FilledSurvey (
    SurveyID INT,
    Question VARCHAR(100),
    LearnerID INT,
    Answer VARCHAR(50),
    PRIMARY KEY (SurveyID, Question, LearnerID),
  FOREIGN KEY (SurveyID, Question) REFERENCES SurveyQuestions(SurveyID, Question)on delete cascade on update cascade,
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade


  
);

--29
CREATE TABLE Notification (
    ID INT PRIMARY KEY identity,
    timestamp DATETIME,
    message VARCHAR(100),
    urgency_level VARCHAR(50)
);

-- 30
CREATE TABLE ReceivedNotification (
    NotificationID INT,
    LearnerID INT,
    PRIMARY KEY (NotificationID, LearnerID),
    FOREIGN KEY (NotificationID) REFERENCES Notification(ID)on delete cascade on update cascade,
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);

--31
CREATE TABLE Badge (
    BadgeID INT PRIMARY KEY identity,
    title VARCHAR(100),
    description VARCHAR(100),
    criteria VARCHAR(100),
    points INT
);

-- 32
CREATE TABLE SkillProgression (
    ID INT PRIMARY KEY identity,
    proficiency_level VARCHAR(50),
    LearnerID INT,
    skill_name VARCHAR(50),
    timestamp DATETIME,
    FOREIGN KEY (LearnerID, skill_name) REFERENCES Skills(LearnerID, skill)on delete cascade on update cascade
);

-- 33
CREATE TABLE Achievement (
    AchievementID INT PRIMARY KEY identity,
    LearnerID INT,
    BadgeID INT,
    description VARCHAR(100),
    date_earned DATETIME,
    type VARCHAR(50),
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade,
    FOREIGN KEY (BadgeID) REFERENCES Badge(BadgeID)on delete cascade on update cascade
);

--34
CREATE TABLE Reward (
    RewardID INT PRIMARY KEY identity,
    value DECIMAL(10, 2),
    description VARCHAR(100),
    type VARCHAR(50)
);

-- 35
CREATE TABLE Quest (
    QuestID INT PRIMARY KEY identity,
    difficulty_level VARCHAR(50),
    criteria VARCHAR(100),
    description VARCHAR(100),
    title VARCHAR(50)
);

-- 36
CREATE TABLE Skill_Mastery (
    QuestID INT,
    skill VARCHAR(50),
    PRIMARY KEY (QuestID, skill),
    FOREIGN KEY (QuestID) REFERENCES Quest(QuestID)on delete cascade on update cascade
);

-- 37
CREATE TABLE Collaborative (
    QuestID INT,
    deadline DATETIME,
    max_num_participants INT,
    PRIMARY KEY (QuestID),
    FOREIGN KEY (QuestID) REFERENCES Quest(QuestID)on delete cascade on update cascade
);
--38
create table LearnersCollaboration(
    LearnerID int,
    QuestID int,
    completion_status nvarchar(MAX),
    Primary key(LearnerID,QuestID),
    foreign key(LearnerID) references Learner(LearnerID) ,
    foreign key(QuestID) references Collaborative(QuestID)
);
--39
create table LearnerMastery(
    LearnerID int,
    QuestID int,
    skill varchar(50),
    completion_status nvarchar(MAX),
    Primary key(LearnerID,QuestID,skill),
    foreign key(LearnerID) references Learner(LearnerID) ,
    foreign key(QuestID,skill) references Skill_Mastery(QuestID,skill)
);
--40
CREATE TABLE Discussion_forum (
    forumID INT PRIMARY KEY identity,
    ModuleID INT,
    CourseID INT,
    title VARCHAR(50),
    last_active DATETIME,
    timestamp DATETIME,
    description VARCHAR(100),
    FOREIGN KEY (ModuleID, CourseID) REFERENCES Modules(ModuleID, CourseID)on delete cascade on update cascade
);

-- 41
CREATE TABLE LearnerDiscussion (
    ForumID INT,
    LearnerID INT,
    Post VARCHAR(100),
    time DATETIME,
    PRIMARY KEY (ForumID, LearnerID),
    FOREIGN KEY (ForumID) REFERENCES Discussion_forum(forumID)on delete cascade on update cascade,
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);

-- 42
CREATE TABLE QuestReward (
    RewardID INT,
    QuestID INT,
    LearnerID INT,
    Time_earned DATETIME,
    PRIMARY KEY (RewardID, QuestID, LearnerID),
    FOREIGN KEY (RewardID) REFERENCES Reward(RewardID)on delete cascade on update cascade,
    FOREIGN KEY (QuestID) REFERENCES Quest(QuestID)on delete cascade on update cascade,
    FOREIGN KEY (LearnerID) REFERENCES Learner(LearnerID)on delete cascade on update cascade
);
--43
create table takesassesment(
    learner_id int,
    assesment_id int,
    ScoredPoints int,
    primary key(learner_id,assesment_id),
    foreign key(learner_id)references Learner on delete cascade on update cascade,
    foreign key(assesment_id)references Assessments on delete cascade on update cascade
);
