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
CREATE PROCEDURE EmotionalState 
	(
		@LearnerID AS INT, 
		@emotional_state AS VARCHAR(50)
	)
AS
BEGIN
	SELECT emotional_state
	FROM Emotional_feedback e
	WHERE e.LearnerID = @LearnerID
END
DROP PROCEDURE EmotionalState
EXEC EmotionalState 1

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

--10
GO 
CREATE PROCEDURE 
AS
BEGIN
	
END

--11
-- Joe

--Ibrahim
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
exec SkillsProficiency 1

--Learner10(relation takesassesment found in ERD but not found in schema)
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
create proc Courseregister(@LearnerID int, @CourseID int )
AS 
begin if(
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
from LearnersGoals
*/
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
--Learner19
Go
create proc SkillProgressHistory(@LearnerID int, @Skill varchar(50))
AS
Begin
select *
from SkillProgression
where LearnerID=@LearnerID AND skill_name=@Skill
End
exec SkillProgressHistory 2,'Graphic Design'

--Learner20
Go 
create proc  AssessmentAnalysis(@LearnerID int, @AssessmentID int)
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