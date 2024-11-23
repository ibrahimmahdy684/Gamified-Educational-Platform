USE GamifiedPlatform

--ADMIN PROCEDURES

-- Maher
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

--6 (how to remove dependancies?)
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

--7 (do i need to display the course itself?)
GO
CREATE PROCEDURE Highestgrade 
AS
BEGIN
	SELECT MAX(total_marks)
	FROM Assessments
	GROUP BY CourseID
END
EXEC Highestgrade

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

--10 (trigger?)(do i need to check if midule or course exists?)
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

--11 (do i need to check if it exists?)
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

-- Joe


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
drop procedure EmotionalTrendAnalysis
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

--1
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
go
create procedure EnrolledCourses
    @LearnerID int
AS
BEGIN
select c.Title ,c.CourseID
from Course_enrollment ce inner join Learner l on l.LearnerID=ce.LearnerID
                          inner join Course c on c.CourseID=ce.CourseID
where l.LearnerID=@LearnerID
end

exec EnrolledCourses 2


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
END;--Ibrahim
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
create proc AssessmentsList(@courseID int,@ModuleID int)
AS
begin
select a.title,t.ScoredPoints
from takesassesment t inner join Assessments a on  t.assesment_id=a.ID 
inner join Learner l on l.LearnerID=t.learner_id
where a.ModuleID=@ModuleID AND a.CourseID=@courseID
end
exec AssessmentsList 2,4

--Learner12
Go
create proc Courseregister( @LearnerID int, @CourseID int)
AS
begin
declare @preq int
declare @preqmet int

select @preq=count(*)
from Prerequisites p
where p.course_id=@CourseID

select @preqmet=COUNT(*)
from Course_enrollment ce inner join Prerequisites p on ce.CourseID=p.prereq
where p.course_id=@CourseID and ce.LearnerID=@LearnerID and ce.status='Completed'

if @preq=0
Begin 
insert into Course_enrollment(CourseID,LearnerID,enrollment_date,status)
values(@CourseID,@LearnerID,GETDATE(),'Enrolled')
print 'Registration approved'
end

else if @preqmet=@preq
begin 
insert into Course_enrollment(CourseID,LearnerID,enrollment_date,status)
values(@CourseID,@LearnerID,GETDATE(),'Enrolled')
print 'Registration approved'
end
else 
begin
print 'Registeration Failed,You did not complete all prerequisites of this course'
end
end
exec Courseregister 3,5
--Learner13
Go

create proc Post(@LearnerID int,@DiscussionID int,@Post varchar(max))
AS
begin
insert into LearnerDiscussion(ForumID,LearnerID,Post) values (@LearnerID,@DiscussionID,@Post);
end
exec Post 7,4,'test'
/*test
select *
from LearnerDiscussion*/
--Learner14
Go
create proc AddGoal(@LearnerID int, @GoalID int)
As 
Begin 
insert into LearnersGoals(GoalID,LearnerID)values(@LearnerID,@GoalID)
End
exec AddGoal 4,5
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
Go 
create proc  AssessmentAnalysis(@LearnerID int)
AS 
begin
select a.title as Assessment,t.ScoredPoints as grade
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
-- Darwish

--INSTRUCTOR PROCEDURES

-- Mariam
--9 (what value to update to?)
GO
CREATE PROCEDURE GradeUpdate(@LearnerID AS int, @AssessmentID AS int)
AS
BEGIN
    UPDATE takesassesment
    SET ScoredPoints = 
    WHERE learner_id=@LearnerID AND assesment_id=@AssessmentID
END

--10 (notification id is identity)
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

--11 (goal id is identity)
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

--12 (what is the purpose of courseid?)
GO
CREATE PROCEDURE LearnersCourses
(
    @CourseID AS INT, 
    @InstructorID AS INT
)
AS
BEGIN
    SELECT 
        ce.LearnerID,
        ce.CourseID
    FROM 
        Course_enrollment ce
    INNER JOIN 
        Teaches t
        ON ce.CourseID = t.CourseID
    WHERE 
        t.InstructorID = @InstructorID
        AND (@CourseID = @CourseID);
END

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

--16 (profeciency level is not numeric)
GO
CREATE PROCEDURE Profeciencylevel(@LearnerID AS int, @skill AS varchar(50) Output)
AS
BEGIN
    SELECT TOP 1 skill_name
    FROM SkillProgression
    WHERE LearnerID=@LearnerID
    ORDER BY proficiency_level DESC
END
DECLARE @skill VARCHAR(50)
EXEC Profeciencylevel 2, @skill = @skill OUTPUT
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
GO
CREATE PROCEDURE AssessmentAnalytics(@CourseID AS int, @ModuleID AS int)
AS
BEGIN
    SELECT 
        a.ID AS AssessmentID,
        a.title AS AssessmentTitle,
        a.total_marks,
        a.passing_marks,
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
drop procedure AssessmentAnalytics
EXEC AssessmentAnalytics 1, 1
/* testing
SELECT * from takesassesment
Select * from Assessments*/

--21
GO
CREATE PROCEDURE EmotionalTrendAnalysis(@CourseID AS int, @ModuleID AS int, @TimePeriod AS varchar(50))
AS
BEGIN

END