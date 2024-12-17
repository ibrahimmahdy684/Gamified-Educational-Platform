USE GamifiedPlatform

Go
create proc PostINS(@instructorID int,@DiscussionID int,@Post varchar(max))
AS
begin
insert into InstructorDiscussion(ForumID,InstructorID,Post,time) values (@DiscussionID,@instructorID,@Post,GETDATE());
end

Go
create proc markAsRead(@notificationID int)
AS
begin
update Notification
set urgency_level='read'
where ID=@notificationID
End
	
GO
CREATE PROCEDURE ViewNotAdmin (@adminID AS INT)
AS
BEGIN
	SELECT Notification.ID, Notification.message, Notification.urgency_level, Notification.timestamp
	FROM Notification
	INNER JOIN 
		ReceivedNotification ON ReceivedNotification.NotificationID = Notification.ID AND 
		ReceivedNotification.adminID = @adminID
END
	
GO
CREATE PROCEDURE DeleteLearner
    @LearnerID INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Example manual deletions
        DELETE FROM LearnerMastery WHERE LearnerID = @LearnerID;
        DELETE FROM LearnersCollaboration WHERE LearnerID = @LearnerID;

        -- Cascade delete handled here
        DELETE FROM Learner WHERE LearnerID = @LearnerID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

GO
CREATE PROCEDURE updateLearnerInfo
    @learnerID INT,
    @firstName VARCHAR(50),
    @lastName VARCHAR(50),
    @country VARCHAR(50),
    @email VARCHAR(50),
    @cultural_background VARCHAR(50),
    @profilePicturePath NVARCHAR(MAX) -- Add parameter for profile picture
AS
BEGIN
    UPDATE Learner
    SET 
        first_name = @firstName,
        last_name = @lastName,
        country = @country,
        email = @email,
        cultural_background = @cultural_background,
        ProfilePicturePath = @profilePicturePath -- Update profile picture
    WHERE LearnerID = @learnerID
END
GO

Go 
create proc getCurrentLearnerPassword(@learnerID int,@password varchar(50) output)
AS
begin
select @password=u.Password
from Learner l inner join Users u on l.UserID=u.UserID
where l.LearnerID=@learnerID
end
	 
Go 
create proc AllLearnersInfo
AS
begin
select*
from PersonalizationProfiles
end
exec AllLearnersInfo

Go
create proc getAllForums
AS
begin
select*
from Discussion_forum
end
exec getAllForums

Go
create proc monitorSpecificPath(@learnerID int)
AS
begin 
select*
from Learning_path
where LearnerID=@learnerID
end
drop proc monitorSpecificPath
Go
create proc getUserInfo(@UserID int)
AS
begin
select*
from users
where UserID=@UserID
END

Go
create proc updateUserInfo(@UserID int,@name varchar,@email varchar,@phone varchar,@address varchar)
AS
begin
update users
set Name=@name,Email=@email,Phone=@phone,Address=@address
where UserID=@UserID
End

drop procedure updateInstructorInfo

GO
CREATE PROCEDURE updateInstructorInfo
    @InstructorID INT,
    @Name VARCHAR(50) = NULL,
    @LatestQualification VARCHAR(50) = NULL,
    @ExpertiseArea VARCHAR(50) = NULL,
    @Email VARCHAR(50) = NULL,
    @ProfilePicturePath VARCHAR(100) = NULL
AS
BEGIN
    UPDATE Instructor
    SET 
        Name = ISNULL(@Name, Name),
        Latest_Qualification = ISNULL(@LatestQualification, Latest_Qualification),
        Expertise_Area = ISNULL(@ExpertiseArea, Expertise_Area),
        Email = ISNULL(@Email, Email),
        ProfilePicturePath = ISNULL(@ProfilePicturePath, ProfilePicturePath)
    WHERE InstructorID = @InstructorID;
END

GO
CREATE PROCEDURE ViewPersonalizationProfiles 
(
    @LearnerID AS INT
)
AS
BEGIN
    SELECT *
    FROM PersonalizationProfiles p
    WHERE p.LearnerID = @LearnerID
END

EXEC ViewPersonalizationProfiles 7

GO
CREATE PROCEDURE DeletePersonalizationProfile 
(
    @ProfileID AS INT
)
AS
BEGIN
    DELETE 
    FROM PersonalizationProfiles
    WHERE PersonalizationProfiles.ProfileID = @ProfileID
END

select * from PersonalizationProfiles
exec DeletePersonalizationProfile 104

------ADMIN PROCEDURES----------
--1
GO
CREATE PROCEDURE ViewInfo (@LearnerID AS INT)
AS
BEGIN
	SELECT *
	FROM Learner
	WHERE @LearnerID = Learner.LearnerID
END
EXEC ViewInfo 4

--2
GO
CREATE PROCEDURE LearnerInfo (@LearnerID AS INT)
AS 
BEGIN
	SELECT *
	FROM PersonalizationProfiles
	WHERE @LearnerID = PersonalizationProfiles.LearnerID 
END
EXEC LearnerInfo 3

--3
GO
CREATE PROCEDURE EmotionalState (@LearnerID INT)
AS
BEGIN
	SELECT TOP 1 emotional_state
	FROM Emotional_feedback e
	WHERE e.LearnerID = @LearnerID
	ORDER BY timestamp DESC
END
EXEC EmotionalState 2

--4
GO 
CREATE PROCEDURE LogDetails (@LearnerID AS INT)
AS
BEGIN
	SELECT *
	FROM Interaction_log i
	WHERE i.LearnerID = @LearnerID
END
EXEC LogDetails 1

--5
GO
CREATE PROCEDURE InstructorReview (@InstructorID AS INT)
AS
BEGIN
	SELECT *
	FROM Emotionalfeedback_review e
	WHERE e.InstructorID = @InstructorID
END
EXEC InstructorReview 2

--6 
GO
CREATE PROCEDURE CourseRemove (@courseID AS INT)
AS
BEGIN
	DELETE FROM Course
	WHERE CourseID = @courseID
END
EXEC CourseRemove 10
/* testing
select * from Course
INSERT INTO Course (CourseID, Title, learning_objective, credit_points, difficulty_level, pre_requisites, description)
VALUES (10, 'Introduction to Programming', 'Learn the basics of coding', 4, 'Beginner', 'None', 'A beginner-level course.')*/

--7 
drop procedure Highestgrade
go
CREATE PROCEDURE HighestGrade
    @CourseId AS INT
AS
BEGIN
    SELECT TOP 1
        a.ID AS AssessmentId,         -- Ensure this matches the model property name
        a.Title AS AssessmentTitle,
        a.total_marks AS HighestGrade               
    FROM Assessments a
    WHERE a.CourseID = @CourseId
    ORDER BY a.total_marks DESC
END


EXEC Highestgrade 1

--8 (how no output?)
GO
CREATE PROCEDURE InstructorCount
AS
BEGIN
	SELECT *
	FROM Course
END

--9
GO
CREATE PROCEDURE ViewNot (@LearnerID AS INT)
AS
BEGIN
	SELECT Notification.ID, Notification.message, Notification.urgency_level, Notification.timestamp
	FROM Notification
	INNER JOIN 
		ReceivedNotification ON ReceivedNotification.NotificationID = Notification.ID AND 
		ReceivedNotification.LearnerID = @LearnerID
END
EXEC ViewNot 2

--10 
GO 
CREATE PROCEDURE CreateDiscussion 
(
	@ModuleID AS int, @courseID AS int, @title AS varchar(50), @description AS varchar(50)
)
AS
BEGIN
	INSERT INTO Discussion_forum (ModuleID, CourseID, title, last_active, timestamp, description)
		VALUES
		(@ModuleID, @courseID, @title, GETDATE(), GETDATE(), @description)

	print 'Discussion forum created successfully for ModuleID: ' + 
		CAST(@ModuleID AS VARCHAR) + ' and CourseID: ' + CAST(@CourseID AS VARCHAR);
END
SELECT * FROM Discussion_forum
EXEC CreateDiscussion 1, 1, 'TEST', 'test description'

--11 
GO
CREATE PROCEDURE RemoveBadge (@BadgeID AS INT)
AS
BEGIN
	DELETE FROM Badge
	WHERE BadgeID = @BadgeID

	print 'Deleted the following badge: ' + CAST(@BadgeID AS VARCHAR);
END
EXEC RemoveBadge 8
/* testing
SELECT * FROM Badge
INSERT INTO Badge ( title, description, criteria, points) VALUES
( 'test', 'test', 'test', 10)*/

--12
go 
create procedure CriteriaDelete
@criteria varchar(50)
as
begin 
delete from Quest 
where criteria=@criteria
end;
exec CriteriaDelete 'Complete a beginner course'

--13
go
create procedure NotificationUpdate
@LearnerID int,
@NotificationID int,
@ReadStatus bit
as
begin
if @ReadStatus=1
begin
update Notification
set urgency_level='read'
where ID = @NotificationID
and exists (
  select 1
  from ReceivedNotification
  where LearnerID = @LearnerID AND NotificationID = @NotificationID
  );
print 'notfication is read';
end
else
begin
delete from ReceivedNotification
where LearnerID = @LearnerID and NotificationID = @NotificationID;
print 'notfication deleted';
end
end;

exec NotificationUpdate 2 ,2 ,1

--14

go 
create procedure EmotionalTrendAnalysis
 @CourseID int, @ModuleID int, @TimePeriod datetime
 as
 begin 
 select ef.emotional_state,l.LearnerID
 from  Emotional_feedback ef inner join Learner l on l.LearnerID=ef.LearnerID
                             inner join Course_enrollment ce on ce.LearnerID=l.LearnerID
                             inner join Modules m on ce.CourseID=m.CourseID
                             inner join Course c  on m.CourseID=m.CourseID

where c.CourseID=@CourseID and m.ModuleID = @ModuleID and ce.enrollment_date<=@TimePeriod and ce.completion_date>=@TimePeriod
end;
exec EmotionalTrendAnalysis 3,2,'2024-11-10 11:30:00'

-----------LEARNER PROCEDURES-----------

--1
GO
CREATE PROCEDURE ProfileUpdate
    @LearnerID INT,
    @ProfileID INT,
    @PreferedContentType VARCHAR(50),
    @emotional_state VARCHAR(50),
    @PersonalityType VARCHAR(50)
AS
BEGIN

        -- Update the profile with the new details
        UPDATE PersonalizationProfiles
        SET 
            Prefered_content_type = @PreferedContentType,
            emotional_state = @emotional_state,
            personality_type = @PersonalityType
        WHERE LearnerID = @LearnerID AND ProfileID = @ProfileID;

        
  
END;
exec ProfileUpdate  1,101,'speaking','sad','shy'

--2
go
create procedure TotalPoints
@LearnerID int, @RewardType varchar(50)
as
begin
select sum(r.value)
from Reward r inner join QuestReward qr on r.RewardID=qr.RewardID

where qr.LearnerID=@LearnerID and r.type=@Rewardtype
end 

exec TotalPoints 4,'Points'

--3
drop procedure EnrolledCourses
GO
CREATE PROCEDURE EnrolledCourses
    @LearnerID INT
AS
BEGIN
    SELECT 
        c.CourseID, 
        c.Title, 
        c.learning_objective, 
        c.credit_points, 
        c.description, 
        c.difficulty_level, 
        c.pre_requisites, 
        ce.status,
        ISNULL(ha.HighestAssessmentGrade, 0) AS HighestAssessmentGrade, -- Use ISNULL to handle NULLs
        ha.HighestAssessmentTitle, 
        ha.HighestAssessmentId
    FROM Course_enrollment ce
    INNER JOIN Learner l ON l.LearnerID = ce.LearnerID
    INNER JOIN Course c ON c.CourseID = ce.CourseID
    LEFT JOIN (
        SELECT 
            ranked.CourseID, 
            ranked.total_marks AS HighestAssessmentGrade, 
            ranked.Title AS HighestAssessmentTitle, 
            ranked.ID AS HighestAssessmentId
        FROM (
            SELECT 
                a.CourseID, 
                a.total_marks, 
                a.Title, 
                a.ID, 
                ROW_NUMBER() OVER (PARTITION BY a.CourseID ORDER BY a.total_marks DESC) AS Rank
            FROM Assessments a
        ) ranked
        WHERE ranked.Rank = 1
    ) ha ON ha.CourseID = c.CourseID
    WHERE l.LearnerID = @LearnerID
END
GO

exec EnrolledCourses 1

--4
GO
CREATE PROCEDURE Prerequisites
    @LearnerID INT,
    @CourseID INT,
    @StatusMessage VARCHAR(100) OUTPUT
AS
BEGIN
    
    DECLARE @Prerequisites VARCHAR(100);
    SELECT @Prerequisites = pre_requisites
    FROM Course
    WHERE CourseID = @CourseID;

    IF @Prerequisites IS NULL OR @Prerequisites = ''
    BEGIN
        SET @StatusMessage = 'No prerequisites required for this course.';
        RETURN;
    END

    -- Check if all prerequisites are completed by the learner
    IF NOT EXISTS (
        SELECT 1
        FROM Course_enrollment ce
        WHERE ce.LearnerID = @LearnerID
          AND ce.CourseID IN (SELECT value FROM STRING_SPLIT(@Prerequisites, ','))
          AND ce.status = 'Completed'
    )
    BEGIN
        SET @StatusMessage = 'Prerequisites are not yet completed.';
    END
    ELSE
    BEGIN
        SET @StatusMessage = 'All prerequisites are completed.';
    END
END;
GO

--5
go
create procedure Moduletraits
@TargetTrait varchar(50), 
@CourseID int
as
begin
select m.Title ,m.ModuleID 
from Modules m inner join Target_traits tt ON m.ModuleID = tt.ModuleID AND m.CourseID = tt.CourseID    
where tt.Trait = @TargetTrait  AND m.CourseID = @CourseID;
end;
exec Moduletraits 'Creativity',3

--6
go
create procedure LeaderboardRank
@LeaderboardID int
as
begin 
select l.first_name,l.last_name ,r.rank,r.total_points
from Learner l inner join Ranking r on l.LearnerID=r.LearnerID
where r.BoardID=@LeaderboardID
order by r.rank
end

exec LeaderboardRank 4

--7
GO
CREATE PROCEDURE ViewMyDeviceCharge
    @ActivityID INT,
    @LearnerID INT,
    @timestamp TIME,
    @emotionalstate VARCHAR(50)
AS
BEGIN
    -- Check if the activity exists
    IF EXISTS (
        SELECT 1
        FROM Learning_activities
        WHERE ActivityID = @ActivityID
    )
    BEGIN
        -- Check if the learner exists
        IF EXISTS (
            SELECT 1
            FROM Learner
            WHERE LearnerID = @LearnerID
        )
        BEGIN
            -- Insert emotional feedback
            INSERT INTO Emotional_feedback (FeedbackID, LearnerID, timestamp, emotional_state)
            VALUES (
                (SELECT ISNULL(MAX(FeedbackID), 0) + 1 FROM Emotional_feedback), -- Generate new FeedbackID
                @LearnerID,
                GETDATE(),
                @emotionalstate
            );

            PRINT 'Emotional feedback submitted successfully.';
        END
        ELSE
        BEGIN
            PRINT 'Error: Learner does not exist.';
        END
    END
    ELSE
    BEGIN
        PRINT 'Error: Activity does not exist.';
    END
END;


--8
go
CREATE PROCEDURE JoinQuest
    @LearnerID INT,
    @QuestID INT
AS
BEGIN
    DECLARE @MaxParticipants INT;
    DECLARE @CurrentParticipants INT;

    -- Get the maximum number of participants for the quest
    SELECT @MaxParticipants = max_num_participants
    FROM Collaborative
    WHERE QuestID = @QuestID;

    -- Check if the quest exists
    IF @MaxParticipants IS NULL
    BEGIN
        PRINT 'Error: Quest does not exist.';
        RETURN;
    END

    -- Count current participants for the quest
    SELECT @CurrentParticipants = COUNT(*)
    FROM QuestReward
    WHERE QuestID = @QuestID;

    -- Check if space is available
    IF @CurrentParticipants < @MaxParticipants
    BEGIN
        -- Add the learner to the quest
        INSERT INTO QuestReward (RewardID, QuestID, LearnerID, Time_earned)
        VALUES (
            (SELECT ISNULL(MAX(RewardID), 0) + 1 FROM QuestReward), -- Generate new RewardID
            @QuestID,
            @LearnerID,
            GETDATE()
        );

        PRINT 'Approval: You have successfully joined the quest.';
    END
    ELSE
    BEGIN
        PRINT 'Rejection: No space available in the quest.';
    END
END;

--Learner9
Go
create proc SkillsProfeciency
(@learnerId int)
AS
begin
select skill_name,proficiency_level
from SkillProgression
where LearnerID=@learnerId
end
exec SkillsProfeciency 1

--Learner10
Go
create proc Viewscore(@LearnerID int,@AssessmentID int,@score int output)
AS
begin 
select @score=ScoredPoints
from takesassesment
where learner_id=@LearnerID and @AssessmentID=assesment_id
end
declare @score int
exec Viewscore 1,2, @score output
print @score
--Learner11

Go 
create proc AssessmentsList(@courseID int,@ModuleID int,@LearnerID int)
AS
begin
select a.title,t.ScoredPoints
from takesassesment t inner join Assessments a on  t.assesment_id=a.ID 
inner join Learner l on l.LearnerID=t.learner_id
where a.ModuleID=@ModuleID AND a.CourseID=@courseID AND t.learner_id=@LearnerID
end

exec AssessmentsList 2,4,2

--Learner12
drop procedure Courseregister
GO
CREATE PROCEDURE Courseregister(@LearnerID INT, @CourseID INT)
AS
BEGIN
    DECLARE @preq INT
    DECLARE @preqmet INT

    SELECT @preq = COUNT(*)
    FROM Prerequisites p
    WHERE p.course_id = @CourseID

    SELECT @preqmet = COUNT(*)
    FROM Course_enrollment ce 
    INNER JOIN Prerequisites p ON ce.CourseID = p.prereq
    WHERE p.course_id = @CourseID 
      AND ce.LearnerID = @LearnerID 
      AND ce.status = 'Completed'

    IF @preq = 0
    BEGIN
        INSERT INTO Course_enrollment (CourseID, LearnerID, enrollment_date, status)
        VALUES (@CourseID, @LearnerID, GETDATE(), 'Enrolled')
        SELECT 'Registration approved' AS Message
    END
    ELSE IF @preqmet = @preq
    BEGIN
        INSERT INTO Course_enrollment (CourseID, LearnerID, enrollment_date, status)
        VALUES (@CourseID, @LearnerID, GETDATE(), 'Enrolled')
        SELECT 'Registration approved' AS Message
    END
    ELSE
    BEGIN
        SELECT 'Registration Failed, You did not complete all prerequisites of this course' AS Message
    END
END

drop procedure Courseregister
exec Courseregister 6,5
--Learner13
Go

create proc Post(@LearnerID int,@DiscussionID int,@Post varchar(max))
AS
begin
insert into LearnerDiscussion(ForumID,LearnerID,Post,time) values (@DiscussionID,@LearnerID,@Post,GETDATE());
end
exec Post 6,5,'hhh'
/*test
select *
from LearnerDiscussion*/

--Learner14
Go
create proc AddGoal(@LearnerID int, @GoalID int)
As 
Begin 
insert into LearnersGoals(GoalID,LearnerID)values(@GoalID,@LearnerID)
End
exec AddGoal 4,2
/*test
select *
from LearnersGoals*/

--Learner 15
Go 
create proc CurrentPath(@LearnerID int)
As
Begin
select *
from Learning_path
where LearnerID=@LearnerID
end
exec CurrentPath 1

--Learner16
Go
create proc QuestMembers(@LearnerID int )
AS
Begin
select l.first_name,l.last_name,c.QuestID
from LearnersCollaboration lc inner join Collaborative c on lc.QuestID=c.QuestID
inner join LearnersCollaboration other on lc.QuestID=other.QuestID 
inner join Learner l on other.LearnerID=l.LearnerID
where lc.LearnerID=@LearnerID AND c.deadline>GETDATE() 
end

exec QuestMembers 1

--Learner17
Go
create proc QuestProgress (@LearnerID INT)
AS
BEGIN
    
    SELECT 
        Q.QuestID,
        Q.title AS QuestTitle,
        C.deadline AS deadline,
        LC.completion_status AS QuestCompletionStatus
    FROM Collaborative C inner join LearnersCollaboration LC ON C.QuestID = LC.QuestID
    inner join Quest Q ON C.QuestID = Q.QuestID
    WHERE LC.LearnerID = @LearnerID AND C.deadline > GETDATE() 
    

    
    SELECT 
        B.BadgeID,
        B.title AS BadgeTitle,
        A.date_earned AS DateEarned,
        B.criteria AS BadgeCriteria
    FROM Achievement A inner join Badge B ON A.BadgeID = B.BadgeID
    WHERE A.LearnerID = @LearnerID
    
END
exec QuestProgress 1
--Learner18
Go
create proc GoalReminder( @LearnerID int)
AS
begin
declare @remdays int
declare @goaldesc varchar(max)
select @remdays=DateDiff(day,getDate(),deadline),@goaldesc=g.description
from Learner l inner join LearnersGoals lg on l.LearnerID=lg.LearnerID inner join Learning_goal g on lg.GoalID=g.ID
where l.LearnerID=@LearnerID 

if(@remdays<7)
begin
insert into Notification(timestamp,message,urgency_level)
values(GETDATE(),'You are failing behind on '+@goaldesc,'high')
end
end
exec GoalReminder 1
/*TEST
select*
from Notification*/


--Learner19
Go
CREATE PROCEDURE SkillProgressHistory (
    @LearnerID INT,
    @Skill VARCHAR(50)
)
AS
BEGIN
    
    SELECT 
        SP.timestamp AS ProgressDate,
        SP.proficiency_level AS SkillLevel
    FROM SkillProgression SP
    inner join Skills S ON SP.LearnerID = S.LearnerID AND SP.skill_name = S.skill
    WHERE SP.LearnerID = @LearnerID
      AND SP.skill_name = @Skill
    ORDER BY SP.timestamp ASC;
END;
exec SkillProgressHistory 2,'Graphic Design'

--Learner20
drop procedure AssessmentAnalysis
Go 
create proc  AssessmentAnalysis(@LearnerID int)
AS 
begin
select a.ID, a.title, a.description, a.CourseID, a.ModuleID, a.title as Assessment, a.type,  t.ScoredPoints as grade,
a.total_marks, a.passing_marks, a.criteria, a.weightage
from takesassesment t inner join Assessments a on t.assesment_id=a.ID
where t.learner_id=@LearnerID
end
exec AssessmentAnalysis 1
--Learner21
Go
create proc  LeaderboardFilter(@LearnerID int)
AS
Begin 
select l.BoardID,l.season,c.Title as course,r.total_points,r.rank
from Leaderboard l inner join Ranking r on l.BoardID=r.BoardID inner join Course c on r.CourseID=c.CourseID
where LearnerID=@LearnerID 
order by rank desc
end
 
exec LeaderboardFilter 2
-----------INSTRUCTOR PROCEDURES-----------

GO
CREATE PROCEDURE SkillLearners (@Skillname VARCHAR(50))
AS
BEGIN
    SELECT 
        s.skill,
        l.LearnerID,
        l.first_name,
        l.last_name
    FROM 
        Skills s
    JOIN 
        Learner l ON s.LearnerID = l.LearnerID
    WHERE 
        s.skill = @Skillname;
END;
GO
EXEC SkillLearners @Skillname = 'Leadership';

--2
drop procedure NewActivity
go
CREATE PROCEDURE NewActivity 
(
    @CourseID INT, 
    @ModuleID INT, 
    @activitytype VARCHAR(50), 
    @instructiondetails VARCHAR(MAX),
    @maxpoints INT
)
AS
BEGIN
    INSERT INTO Learning_activities 
    (CourseID, ModuleID, activity_type, instruction_details, Max_points)
    VALUES 
    (@CourseID, @ModuleID, @activitytype, @instructiondetails, @maxpoints);
END;

EXEC NewActivity 
    @CourseID = 1,  
    @ModuleID = 2,  
    @activitytype = 'Quiz',  
    @instructiondetails = 'Complete the quiz by answering all questions correctly.',  
    @maxpoints = 20;  

--3
DROP PROCEDURE IF EXISTS NewAchievement;
GO

CREATE PROCEDURE NewAchievement
(
    @LearnerID INT, 
    @BadgeID INT, 
    @description VARCHAR(MAX), 
    @date_earned DATE, 
    @type VARCHAR(50)
)
AS
BEGIN
    INSERT INTO Achievement 
    (LearnerID, BadgeID, description, date_earned, type)
    VALUES 
    (@LearnerID, @BadgeID, @description, @date_earned, @type);
    
    PRINT 'Achievement awarded successfully!';
END;
GO
EXEC NewAchievement 
    @LearnerID = 1, 
    @BadgeID = 1, 
    @description = 'Awarded for completing the Python course.', 
    @date_earned = '2024-11-23', 
    @type = 'Course Completion';

--4
GO
CREATE PROCEDURE LearnerBadge 
    @BadgeID INT
AS
BEGIN
    SELECT 
        l.LearnerID,
        l.first_name,
        l.last_name,
        a.date_earned,
        b.title AS BadgeTitle
    FROM 
        Achievement a
    JOIN 
        Learner l ON a.LearnerID = l.LearnerID
    JOIN 
        Badge b ON a.BadgeID = b.BadgeID
    WHERE 
        a.BadgeID = @BadgeID
    ORDER BY 
        l.last_name, l.first_name;
END;
GO

EXEC LearnerBadge @BadgeID = 1;  

--5
GO
CREATE PROCEDURE NewPath
(
    @LearnerID INT, 
    @ProfileID INT, 
    @completion_status VARCHAR(50), 
    @custom_content VARCHAR(MAX), 
    @adaptiverules VARCHAR(MAX)
)
AS
BEGIN
    INSERT INTO Learning_path
    (LearnerID, ProfileID, completion_status, custom_content, adaptive_rules)
    VALUES
    (@LearnerID, @ProfileID, @completion_status, @custom_content, @adaptiverules);
    
    PRINT 'Learning path added successfully!';
END;
GO
EXEC NewPath 
    @LearnerID = 1, 
    @ProfileID = 101, 
    @completion_status = 'In Progress', 
    @custom_content = 'Extra tutorials on Python basics.', 
    @adaptiverules = 'Adaptive learning based on quiz scores.';

--6
GO
CREATE PROCEDURE TakenCourses (@LearnerID INT)
AS
BEGIN
    SELECT 
        c.CourseID, 
        c.Title, 
        c.learning_objective, 
        ce.status
    FROM 
        Course c
    JOIN 
        Course_enrollment ce ON c.CourseID = ce.CourseID
    WHERE 
        ce.LearnerID = @LearnerID;
END;
GO
EXEC TakenCourses @LearnerID = 1;  

--7
GO
CREATE PROCEDURE CollaborativeQuest
(
    @difficulty_level VARCHAR(50), 
    @criteria VARCHAR(50), 
    @description VARCHAR(50), 
    @title VARCHAR(50), 
    @Maxnumparticipants INT, 
    @deadline DATETIME
)
AS
BEGIN
  
    INSERT INTO Quest 
    (difficulty_level, criteria, description, title)
    VALUES
    (@difficulty_level, @criteria, @description, @title);
    
   
    DECLARE @QuestID INT = SCOPE_IDENTITY();
    
    
    INSERT INTO Collaborative 
    (QuestID, deadline, max_num_participants)
    VALUES 
    (@QuestID, @deadline, @Maxnumparticipants);
    
   
   
END;
GO
EXEC CollaborativeQuest 
    @difficulty_level = 'Hard', 
    @criteria = 'Complete 5 challenges', 
    @description = 'A challenging quest to test your skills.', 
    @title = 'Ultimate Challenge', 
    @Maxnumparticipants = 10, 
    @deadline = '2024-12-31';

--8
GO
CREATE PROCEDURE DeadlineUpdate
(
    @QuestID INT, 
    @deadline DATETIME
)
AS
BEGIN
    
    UPDATE Collaborative
    SET deadline = @deadline
    WHERE QuestID = @QuestID;
   
    PRINT 'Quest deadline updated successfully!';
END;
GO
EXEC DeadlineUpdate 
    @QuestID = 1, 
    @deadline = '2024-12-31 23:59:59';

--9 
GO
CREATE PROCEDURE GradeUpdate(@LearnerID AS int, @AssessmentID AS int, @points INT)
AS
BEGIN
    UPDATE takesassesment
    SET ScoredPoints = @points
    WHERE learner_id=@LearnerID AND assesment_id=@AssessmentID
END
EXEC GradeUpdate 1, 2, 12
--select * from takesassesment

--10
GO
CREATE PROCEDURE AssessmentNot
( 
    @NotificationID AS int, 
    @timestamp AS DATETIME, 
    @message AS varchar(max), 
    @urgencylevel AS varchar(50), 
    @LearnerID AS int
)
AS
BEGIN
     SET IDENTITY_INSERT Notification ON;
     INSERT INTO Notification(ID, timestamp, message, urgency_level)
     VALUES (@NotificationID, @timestamp, @message, @urgencylevel)
     SET IDENTITY_INSERT Notification OFF;

     INSERT INTO ReceivedNotification (NotificationID, LearnerID)
     VALUES (@NotificationID, @LearnerID)

     print 'Sent notification: ' + CAST(@NotificationID as varchar) + 
     ' to learner: ' + CAST(@LearnerID as varchar)
END
EXEC AssessmentNot 15, '2024-11-15 10:00:00', 'test', 'test', 1 
/* testing
Select * from Notification
*/

--11 
GO
CREATE PROCEDURE NewGoal 
(
    @GoalID AS int, 
    @status AS varchar(max), 
    @deadline AS DATETIME, 
    @description AS varchar(max)
)
AS
BEGIN
    SET IDENTITY_INSERT Learning_goal ON;

    INSERT INTO Learning_goal (ID, status, deadline, description) VALUES
    (@GoalID, @status, @deadline, @description)

    SET IDENTITY_INSERT Learning_goal OFF;
END
EXEC NewGoal 15, 'test', '1/1/2005', 'test'
/* testing
select * 
from Learning_goal*/

--12 
drop proc LearnersCourses
GO
CREATE PROCEDURE LearnersCourses
    @CourseID INT,
    @InstructorID INT
AS
BEGIN
    SELECT 
        L.LearnerID,
        L.first_name,
        L.last_name,
        L.gender,
        L.birth_date,
        L.country,
        L.cultural_background,
        C.Title AS CourseTitle
    FROM 
        Course_enrollment CE
    INNER JOIN 
        Learner L ON CE.LearnerID = L.LearnerID
    INNER JOIN 
        Course C ON CE.CourseID = C.CourseID
    INNER JOIN 
        Teaches T ON T.CourseID = C.CourseID
    WHERE 
        T.InstructorID = @InstructorID
        AND (@CourseID IS NULL OR C.CourseID = @CourseID);
END;

exec LearnersCourses 1,1

--13
GO 
CREATE PROCEDURE LastActive(@ForumID AS int, @lastactive AS datetime output)
AS
BEGIN
    SELECT last_active
    FROM Discussion_forum 
    WHERE Discussion_forum.forumID=@ForumID
END

DECLARE @lastactive DATETIME
EXEC LastActive 1, @lastactive = @lastactive OUTPUT;

--14
GO
CREATE PROCEDURE CommonEmotionalState(@state AS varchar(50) output)
AS
BEGIN
    SELECT TOP 1 emotional_state
    FROM Emotional_feedback
    GROUP BY emotional_state
    ORDER BY COUNT(*) DESC
END
DECLARE @state VARCHAR(50)
EXEC CommonEmotionalState @state = @state OUTPUt;
/* testing
INSERT INTO Emotional_feedback ( LearnerID, timestamp, emotional_state) VALUES
( 1, '2024-11-15 10:30:00', 'Happy')
select * from Emotional_feedback*/

--15
GO 
CREATE PROCEDURE ModuleDifficulty(@courseID AS int)
AS
BEGIN
    SELECT *
    FROM Modules 
    WHERE Modules.CourseID = @courseID
    ORDER BY Modules.difficulty
END
EXEC ModuleDifficulty 1

--16 
GO
CREATE PROCEDURE Profeciencylevel(@LearnerID AS int, @skill AS varchar(50) Output)
AS
BEGIN
    SELECT TOP 1 @skill=skill_name
    FROM SkillProgression
    WHERE LearnerID=@LearnerID
    ORDER BY 
        CASE  
            WHEN proficiency_level = 'Expert' THEN 1
            WHEN proficiency_level = 'Advanced' THEN 2
            WHEN proficiency_level = 'Intermediate' THEN 3
            WHEN proficiency_level = 'Beginner' THEN 4
            Else 5
            END
END
DECLARE @skill VARCHAR(50)
EXEC Profeciencylevel 1, @skill = @skill OUTPUT
print @skill
drop procedure Profeciencylevel
/*testing 
SELECT * from SkillProgression
*/

--17
GO 
CREATE PROCEDURE  ProfeciencyUpdate( @Skill AS varchar(50), @LearnerId AS int, @Level AS varchar(50))
AS
BEGIN
    UPDATE SkillProgression 
    SET proficiency_level = @Level
    WHERE skill_name=@Skill AND LearnerId=@LearnerId
END
EXEC ProfeciencyUpdate 'Creative Writing', 5, 'Beginner'
-- testing SELECT * from SkillProgression

--18
GO
CREATE PROCEDURE LeastBadge (@LearnerID AS int Output)
AS
BEGIN
    SELECT TOP 1 LearnerID
    FROM Achievement 
    GROUP BY LearnerID
    ORDER BY COUNT(BadgeID) ASC
END
DECLARE @LearnerID INT
EXEC LeastBadge @LearnerID = @LearnerID OUTPUT
--SELECT * from Achievement

--19
GO 
CREATE PROCEDURE PreferedType(@type AS varchar(50) output)
AS
BEGIN 
    SELECT TOP 1 preference
    FROM LearningPreference
    GROUP BY preference
    ORDER BY Count(LearnerID) DESC
END
DECLARE @type VARCHAR(50)
EXEC PreferedType @type = @type OUTPUT
--Select * from LearningPreference

--20
drop procedure AssessmentAnalytics
GO
CREATE PROCEDURE AssessmentAnalytics(@CourseID AS int, @ModuleID AS int)
AS
BEGIN
    SELECT 
    a.ID AS AssessmentID,
    a.title AS AssessmentTitle,
    a.total_marks AS TotalMarks,
    a.passing_marks AS PassingMarks, -- Ensure this matches the model property
    AVG(t.ScoredPoints) AS AverageScore
FROM Assessments a
INNER JOIN 
    takesassesment t ON a.ID = t.assesment_id
WHERE 
    a.CourseID = @CourseID
    AND a.ModuleID = @ModuleID
GROUP BY 
    a.ID, a.title, a.total_marks, a.passing_marks
ORDER BY 
    a.ID;

END
EXEC AssessmentAnalytics 1, 1
/* testing
SELECT * from takesassesment
Select * from Assessments*/

--21 
drop proc EmotionalTrendAnalysisIns
Go
CREATE PROCEDURE EmotionalTrendAnalysisIns
    @CourseID INT,
    @ModuleID INT,
    @TimePeriod DATETIME
    
AS
BEGIN
    
    SELECT 
        EF.timestamp AS FeedbackTime,
        EF.emotional_state,
        L.first_name+' '+L.last_name AS LearnerName,
        M.Title AS ModuleTitle,
        C.Title AS CourseTitle
    FROM 
        Emotional_feedback EF
    inner join Learner L ON EF.LearnerID = L.LearnerID
    inner join Course_enrollment CE ON L.LearnerID = CE.LearnerID AND CE.CourseID = @CourseID
    inner join Modules M ON M.CourseID = @CourseID AND M.ModuleID = @ModuleID
    inner join Course C ON M.CourseID = C.CourseID
    WHERE 
        EF.timestamp >= @TimePeriod
    
END;
exec EmotionalTrendAnalysisIns 1,1,'2024-11-15 10:30:00'
