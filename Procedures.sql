USE GamifiedPlatform

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
	FROM Interaction_log
	WHERE 
END
EXEC LogDetails
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
select
-- Darwish

-- Mariam
