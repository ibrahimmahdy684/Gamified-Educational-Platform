USE GamifiedPlatform

--Insertion into Users
-- Insert 5 Learners
INSERT INTO Users (Name, Email, Password, Type)
VALUES 
('Learner1', 'learner1@example.com', 'password123', 'Learner'),
('Learner2', 'learner2@example.com', 'password123', 'Learner'),
('Learner3', 'learner3@example.com', 'password123', 'Learner'),
('Learner4', 'learner4@example.com', 'password123', 'Learner'),
('Learner5', 'learner5@example.com', 'password123', 'Learner');

-- Insert 5 Instructors
INSERT INTO Users (Name, Email, Password, Type)
VALUES 
('Instructor1', 'instructor1@example.com', 'password123', 'Instructor'),
('Instructor2', 'instructor2@example.com', 'password123', 'Instructor'),
('Instructor3', 'instructor3@example.com', 'password123', 'Instructor'),
('Instructor4', 'instructor4@example.com', 'password123', 'Instructor'),
('Instructor5', 'instructor5@example.com', 'password123', 'Instructor');
INSERT INTO Users (Name, Email, Password, Type)
VALUES 
('Instructor6', 'instructor6@example.com', 'password123', 'Instructor'),
('Instructor7', 'instructor7@example.com', 'password123', 'Instructor')

-- Insert 5 Admins
INSERT INTO Users (Name, Email, Password, Type)
VALUES 
('Admin1', 'admin1@example.com', 'password123', 'Admin'),
('Admin2', 'admin2@example.com', 'password123', 'Admin'),
('Admin3', 'admin3@example.com', 'password123', 'Admin'),
('Admin4', 'admin4@example.com', 'password123', 'Admin'),
('Admin5', 'admin5@example.com', 'password123', 'Admin');


--Insertion into Admin
-- Insert 5 records into the Admin table
INSERT INTO Admin (UserID, Name, Gender, Birth_Date, Country, Email)
VALUES
((SELECT UserID FROM Users WHERE Email = 'admin1@example.com'), 'Admin1', 'M', '1985-01-15', 'USA', 'admin1@example.com'),
((SELECT UserID FROM Users WHERE Email = 'admin2@example.com'), 'Admin2', 'F', '1990-05-22', 'UK', 'admin2@example.com'),
((SELECT UserID FROM Users WHERE Email = 'admin3@example.com'), 'Admin3', 'M', '1988-03-18', 'Canada', 'admin3@example.com'),
((SELECT UserID FROM Users WHERE Email = 'admin4@example.com'), 'Admin4', 'F', '1992-07-09', 'Australia', 'admin4@example.com'),
((SELECT UserID FROM Users WHERE Email = 'admin5@example.com'), 'Admin5', 'M', '1980-11-30', 'India', 'admin5@example.com');


--Insertion into Learner 
-- Insert 5 records into the Learner table
INSERT INTO Learner (UserID, First_Name, Last_Name, Gender, Birth_Date, Country, Email, Cultural_Background)
VALUES
((SELECT UserID FROM Users WHERE Email = 'learner1@example.com'), 'Learner1', 'Doe', 'M', '2000-04-12', 'USA', 'learner1@example.com', 'American'),
((SELECT UserID FROM Users WHERE Email = 'learner2@example.com'), 'Learner2', 'Smith', 'F', '1999-08-25', 'UK', 'learner2@example.com', 'British'),
((SELECT UserID FROM Users WHERE Email = 'learner3@example.com'), 'Learner3', 'Ahmed', 'M', '1998-01-10', 'Egypt', 'learner3@example.com', 'Middle Eastern'),
((SELECT UserID FROM Users WHERE Email = 'learner4@example.com'), 'Learner4', 'Gonzalez', 'F', '2001-06-15', 'Spain', 'learner4@example.com', 'Hispanic'),
((SELECT UserID FROM Users WHERE Email = 'learner5@example.com'), 'Learner5', 'Zhang', 'M', '1997-03-05', 'China', 'learner5@example.com', 'Asian');


--Insertion into Skills 
INSERT INTO Skills (LearnerID, skill)
VALUES
(1, 'Programming'),
(1, 'Problem Solving'),
(2, 'Graphic Design'),
(2, 'Public Speaking'),
(3, 'Data Analysis'),
(4, 'Leadership'),
(5, 'Creative Writing');

--Insertion into LearningPreference
INSERT INTO LearningPreference (LearnerID, preference)
VALUES
(1, 'Visual'),
(1, 'Kinesthetic'),
(2, 'Auditory'),
(3, 'Interactive'),
(3, 'Self-paced'),
(4, 'Collaborative'),
(5, 'Hands-on');

--Insertion into PersonalizationProfiles 
INSERT INTO PersonalizationProfiles (LearnerID, ProfileID, Prefered_content_type, emotional_state, personality_type)
VALUES 
(1, 101, 'Video', 'Happy', 'Extrovert'),
(2, 102, 'Text', 'Focused', 'Introvert'),
(3, 103, 'Interactive', 'Calm', 'Ambivert'),
(4, 104, 'Audio', 'Excited', 'Extrovert'),
(5, 105, 'Visual', 'Anxious', 'Introvert')
INSERT INTO PersonalizationProfiles (LearnerID, ProfileID, Prefered_content_type, emotional_state, personality_type)
VALUES
(7, 106, 'Text', 'Relaxed', 'Ambivert'),
(7, 107, 'Interactive', 'Motivated', 'Extrovert');

-- Insertion into HealthCondition
INSERT INTO HealthCondition (LearnerID, ProfileID, condition)
VALUES 
(1, 101, 'Asthma'),
(1, 107, 'Allergy'),
(2, 102, 'Diabetes'),
(3, 103, 'Hypertension'),
(3, 106, 'Migraines'),
(4, 104, 'Back Pain'),
(5, 105, 'Anxiety');

--Insertion into Course 
INSERT INTO Course ( Title, learning_objective, credit_points, difficulty_level, pre_requisites, description)
VALUES
('Introduction to Programming', 'Learn the basics of coding', 4, 'Beginner', 'None', 'A beginner-level course.'),
('Advanced Data Analysis', 'Master data analysis techniques', 6, 'Advanced', 'Basic Statistics', 'Focus on real-world datasets.'),
('Graphic Design Basics', 'Understand design principles', 3, 'Intermediate', 'Basic Art Knowledge', 'Hands-on projects.'),
('Public Speaking', 'Enhance communication skills', 2, 'Beginner', 'None', 'Practical speaking exercises.'),
('Creative Writing Workshop', 'Improve writing techniques', 3, 'Intermediate', 'Basic Writing Skills', 'Work on creative projects.'),
('Leadership Fundamentals', 'Develop leadership skills', 4, 'Advanced', 'Team Experience', 'Leadership in practice.'),
('Collaborative Problem Solving', 'Team-based challenges', 5, 'Intermediate', 'None', 'Real-world scenarios.');

-- Insert data into the Prerequisites table
INSERT INTO Prerequisites (course_id, prereq)
VALUES
(2, 1),  -- Course 2 requires Course 1 as a prerequisite
(3, 2),  -- Course 3 requires Course 2 as a prerequisite
(4, 1),  -- Course 4 requires Course 1 as a prerequisite
(5, 3),  -- Course 5 requires Course 3 as a prerequisite
(6, 5);  -- Course 6 requires Course 5 as a prerequisite


-- Insertion into Modules
INSERT INTO Modules (CourseID, Title, difficulty, contentURL)
VALUES 
( 1, 'Introduction to Variables', 'Beginner', 'https://example.com/course1/module1'),
( 1, 'Control Structures in Programming', 'Beginner', 'https://example.com/course1/module2'),
( 1, 'Functions and Loops', 'Beginner', 'https://example.com/course1/module3'),
( 2, 'Data Cleaning and Preparation', 'Advanced', 'https://example.com/course2/module1'),
( 2, 'Exploratory Data Analysis', 'Advanced', 'https://example.com/course2/module2'),
( 2, 'Statistical Modeling', 'Advanced', 'https://example.com/course2/module3'),
( 3, 'Principles of Design', 'Intermediate', 'https://example.com/course3/module1'),
( 3, 'Typography and Color Theory', 'Intermediate', 'https://example.com/course3/module2'),
( 4, 'Public Speaking Techniques', 'Beginner', 'https://example.com/course4/module1'),
( 5, 'Character Development in Writing', 'Intermediate', 'https://example.com/course5/module1');

-- Insertion into Target_traits
INSERT INTO Target_traits (ModuleID, CourseID, Trait)
VALUES
(1, 1, 'Analytical Thinking'),
(2, 1, 'Logical Reasoning'),
(3, 1, 'Problem Solving'),
(4, 2, 'Data Interpretation'),
(5, 2, 'Critical Thinking'),
(6, 2, 'Statistical Analysis'),
(7, 3, 'Creativity'),
(8, 3, 'Attention to Detail'),
(9, 4, 'Confidence'),
(10, 5, 'Imaginative Writing');

-- Insertion into ModuleContent
INSERT INTO ModuleContent (ModuleID, CourseID, content_type)
VALUES
(1, 1, 'Video'),
(2, 1, 'Text'),
(3, 1, 'Interactive'),
(4, 2, 'Text'),
(5, 2, 'Video'),
(6, 2, 'Interactive'),
(7, 3, 'Text'),
(8, 3, 'Video'),
(9, 4, 'Audio'),
(10, 5, 'Text');

-- Insertion into ContentLibrary
INSERT INTO ContentLibrary ( ModuleID, CourseID, Title, description, metadata, type, content_URL)
VALUES
( 1, 1, 'Intro to Variables - Video', 'Introduction to variables in programming', 'Beginner, Programming', 'Video', 'https://example.com/course1/module1/video'),
( 2, 1, 'Control Structures - Text', 'Understanding control structures', 'Beginner, Programming', 'Text', 'https://example.com/course1/module2/text'),
( 3, 1, 'Functions and Loops - Interactive', 'Learn about functions and loops', 'Beginner, Programming', 'Interactive', 'https://example.com/course1/module3/interactive'),
( 4, 2, 'Data Cleaning - Text', 'How to clean data for analysis', 'Advanced, Data Analysis', 'Text', 'https://example.com/course2/module1/text'),
( 5, 2, 'Exploratory Data Analysis - Video', 'Visualizing data insights', 'Advanced, Data Analysis', 'Video', 'https://example.com/course2/module2/video'),
( 6, 2, 'Statistical Modeling - Interactive', 'Learn the basics of statistical modeling', 'Advanced, Data Analysis', 'Interactive', 'https://example.com/course2/module3/interactive'),
( 7, 3, 'Principles of Design - Text', 'Key design principles', 'Intermediate, Graphic Design', 'Text', 'https://example.com/course3/module1/text'),
( 8, 3, 'Typography and Color Theory - Video', 'Learn typography and color theory', 'Intermediate, Graphic Design', 'Video', 'https://example.com/course3/module2/video'),
( 9, 4, 'Public Speaking Techniques - Audio', 'Enhance your public speaking skills', 'Beginner, Public Speaking', 'Audio', 'https://example.com/course4/module1/audio'),
( 10, 5, 'Character Development - Text', 'Tips on writing compelling characters', 'Intermediate, Writing', 'Text', 'https://example.com/course5/module1/text');

-- Insertion into Assessments
INSERT INTO Assessments ( ModuleID, CourseID, type, total_marks, passing_marks, criteria, weightage, description, title)
VALUES
( 1, 1, 'Quiz', 20, 10, 'Multiple choice questions', 20, 'Assessing basic understanding of variables', 'Intro to Variables Quiz'),
( 2, 1, 'Quiz', 30, 15, 'Multiple choice and short answers', 20, 'Testing understanding of control structures', 'Control Structures Quiz'),
( 3, 1, 'Assignment', 50, 25, 'Practical programming exercises', 30, 'Assessment of functions and loops knowledge', 'Functions and Loops Assignment'),
( 4, 2, 'Case Study', 40, 20, 'Analysis of a real-world dataset', 25, 'Assessing skills in data cleaning', 'Data Cleaning Case Study'),
( 5, 2, 'Quiz', 25, 12, 'Multiple choice questions', 15, 'Testing understanding of exploratory data analysis', 'Exploratory Data Analysis Quiz'),
( 6, 2, 'Project', 100, 50, 'Modeling and interpretation of data', 40, 'Assessment of statistical modeling skills', 'Statistical Modeling Project'),
( 7, 3, 'Assignment', 50, 25, 'Designing a logo and layout', 30, 'Practical design skills assessment', 'Principles of Design Assignment'),
( 8, 3, 'Quiz', 30, 15, 'Multiple choice questions', 20, 'Testing understanding of typography and color', 'Typography and Color Theory Quiz'),
( 9, 4, 'Speech', 50, 30, 'Oral presentation', 25, 'Assessment of public speaking skills', 'Public Speaking Techniques Speech'),
( 10, 5, 'Assignment', 40, 20, 'Writing a short story with character development', 30, 'Evaluating creativity and character writing skills', 'Character Development Assignment');


-- Insertion into Learning_activities
INSERT INTO Learning_activities ( ModuleID, CourseID, activity_type, instruction_details, Max_points)
VALUES
( 1, 1, 'Quiz', 'Complete the quiz on variables and data types within 30 minutes.', 20),
( 1, 1, 'Project', 'Create a simple Python program to use variables and print output.', 50),
( 1, 1, 'Exercise', 'Practice using different data types and operators in Python.', 30),
( 2, 1, 'Case Study', 'Analyze a given dataset for outliers and patterns in the data.', 40),
( 2, 1, 'Discussion', 'Participate in a forum discussing the importance of data cleaning.', 15),
( 2, 1, 'Quiz', 'Answer questions on data analysis techniques, focusing on exploratory methods.', 25),
( 7, 3, 'Quiz', 'Answer multiple choice questions on the principles of graphic design.', 20),
( 8, 3, 'Assignment', 'Create a design mockup using typography and color theory principles.', 45),
( 9, 4, 'Assignment', 'Write a script for a public speaking session on leadership.', 30),
( 10, 5, 'Workshop', 'Write a short story and develop characters using creative writing techniques.', 35);

-- Insertion into Interaction_log
INSERT INTO Interaction_log ( activity_ID, LearnerID, Duration, Timestamp, action_type)
VALUES
( 1, 1, '00:20:00', '2024-11-15 09:00:00', 'Started'),
( 1, 2, '00:25:00', '2024-11-15 09:30:00', 'Completed'),
( 2, 1, '01:00:00', '2024-11-15 10:00:00', 'Started'),
( 2, 3, '00:45:00', '2024-11-15 11:00:00', 'Completed'),
( 3, 4, '00:35:00', '2024-11-15 12:00:00', 'Started'),
( 3, 5, '00:50:00', '2024-11-15 12:45:00', 'Completed'),
( 4, 2, '00:40:00', '2024-11-15 14:00:00', 'Started'),
( 4, 5, '00:30:00', '2024-11-15 14:30:00', 'Completed'),
( 5, 1, '00:55:00', '2024-11-15 15:00:00', 'Started'),
( 6, 3, '01:15:00', '2024-11-15 15:30:00', 'Completed');


--Insertion into Emotional_feedback 
INSERT INTO Emotional_feedback ( LearnerID, timestamp, emotional_state)
VALUES
( 1, '2024-11-15 10:30:00', 'Happy'),
( 2, '2024-11-15 11:00:00', 'Focused'),
( 2, '2024-11-15 11:30:00', 'Anxious'),
( 3, '2024-11-15 12:00:00', 'Calm'),
( 4, '2024-11-15 12:30:00', 'Excited'),
( 5, '2024-11-15 13:00:00', 'Motivated'),
( 5, '2024-11-15 13:30:00', 'Relaxed'),
( 5, '2024-11-15 14:00:00', 'Frustrated');

--Insertion into Learning_path 
INSERT INTO Learning_path ( LearnerID, ProfileID, completion_status, custom_content, adaptive_rules)
VALUES
( 1, 101, 'In Progress', 'Python Basics', 'Difficulty Level Adjustment'),
( 2, 102, 'Completed', 'Graphic Design Theory', 'Content Prioritization'),
( 3, 103, 'In Progress', 'Data Analysis Techniques', 'Interactive Learning Path'),
( 4, 104, 'Completed', 'Public Speaking Skills', 'Personalized Feedback'),
( 5, 105, 'In Progress', 'Creative Writing Workshop', 'Timed Assessments'),
( 3, 106, 'Completed', 'Advanced Programming', 'Weekly Milestone Tracking'),
( 1, 107, 'In Progress', 'Data Science Fundamentals', 'Adaptive Learning Path');

--Insertion into Instructor 
-- Insert 5 records into the Instructor table with consistent names from the Users table
INSERT INTO Instructor (UserID, Name, Latest_Qualification, Expertise_Area, Email)
VALUES
((SELECT UserID FROM Users WHERE Email = 'instructor1@example.com'), 'Instructor1', 'PhD in Computer Science', 'Artificial Intelligence', 'instructor1@example.com'),
((SELECT UserID FROM Users WHERE Email = 'instructor2@example.com'), 'Instructor2', 'Masters in Education', 'Pedagogy', 'instructor2@example.com'),
((SELECT UserID FROM Users WHERE Email = 'instructor3@example.com'), 'Instructor3', 'PhD in Physics', 'Quantum Mechanics', 'instructor3@example.com'),
((SELECT UserID FROM Users WHERE Email = 'instructor4@example.com'), 'Instructor4', 'PhD in History', 'Ancient Civilizations', 'instructor4@example.com'),
((SELECT UserID FROM Users WHERE Email = 'instructor5@example.com'), 'Instructor5', 'Masters in Mechanical Engineering', 'Robotics', 'instructor5@example.com');
-- Insert 2 more records into the Instructor table with consistent names from the Users table
INSERT INTO Instructor (UserID, Name, Latest_Qualification, Expertise_Area, Email)
VALUES
((SELECT UserID FROM Users WHERE Email = 'instructor6@example.com'), 'Instructor6', 'Masters in Computer Engineering', 'Software Development', 'instructor6@example.com'),
((SELECT UserID FROM Users WHERE Email = 'instructor7@example.com'), 'Instructor7', 'PhD in Environmental Science', 'Sustainability', 'instructor7@example.com');


--Insertion into Pathreview 
INSERT INTO Pathreview (InstructorID, PathID, feedback)
VALUES
(1, 1, 'Good progress, needs more challenging tasks'),
(2, 2, 'Well done, keep it up!'),
(3, 3, 'Needs more interactive content to engage learners'),
(4, 4, 'Great job, very engaging course material'),
(5, 5, 'Needs more structure in assignments'),
(6, 6, 'Great use of real-world examples, very practical'),
(7, 7, 'Excellent pacing and adaptive learning techniques');

--Insertion into Emotionalfeedback_review 
INSERT INTO Emotionalfeedback_review (FeedbackID, InstructorID, feedback)
VALUES
(1, 1, 'Student seems very happy, great emotional state for learning'),
(2, 2, 'Student is focused, excellent mindset for learning'),
(3, 2, 'Student seems anxious, might need some support'),
(4, 3, 'Student is calm, great for focused learning'),
(5, 4, 'Student is excited, highly motivated to learn'),
(6, 5, 'Student is motivated, keep encouraging them'),
(7, 5, 'Student is relaxed, make sure to keep the engagement');

--Insertion into Course_enrollment 
INSERT INTO Course_enrollment ( CourseID, LearnerID, completion_date, enrollment_date, status)
VALUES
( 1, 1, '2024-12-10', '2024-11-01', 'In Progress'),
( 2, 2, '2024-12-15', '2024-11-05', 'In Progress'),
( 3, 3, '2024-12-20', '2024-11-10', 'Completed'),
( 4, 4, '2024-12-05', '2024-11-03', 'Completed'),
( 5, 5, '2024-12-25', '2024-11-12', 'In Progress'),
( 6, 1, '2024-12-30', '2024-11-15', 'In Progress'),
( 7, 2, '2024-12-18', '2024-11-08', 'Completed');
INSERT INTO Course_enrollment ( CourseID, LearnerID, completion_date, enrollment_date, status)
VALUES
( 1, 6, '2024-12-10', '2024-11-01', 'In Progress')
INSERT INTO Course_enrollment ( CourseID, LearnerID, completion_date, enrollment_date, status)
VALUES
( 7, 6, '2024-12-10', '2024-11-01', 'Completed')


--Insertion into Teaches 
INSERT INTO Teaches (InstructorID, CourseID)
VALUES
(1, 1),
(2, 3),
(3, 2),
(4, 4),
(5, 5),
(6, 6),
(7, 7);

--Insertion into Leaderboard 
INSERT INTO Leaderboard ( season)
VALUES
('2024 Fall'),
('2024 Winter'),
('2025 Spring'),
('2025 Summer'),
('2025 Fall'),
('2026 Winter'),
('2026 Spring');

--Insertion into Ranking 
INSERT INTO Ranking (BoardID, LearnerID, CourseID, rank, total_points)
VALUES
(1, 1, 1, 1, 90),  -- Alice Smith in Introduction to Programming (2024 Fall)
(2, 2, 2, 2, 85),  -- Mohammed Ali in Advanced Data Analysis (2024 Winter)
(3, 3, 3, 3, 80),  -- Wei Zhang in Graphic Design Basics (2025 Spring)
(4, 4, 4, 4, 75),  -- Maria Gonzalez in Public Speaking (2025 Summer)
(5, 5, 5, 5, 70),  -- John Doe in Creative Writing Workshop (2025 Fall)
(6, 1, 6, 6, 65),  -- Alice Smith in Leadership Fundamentals (2026 Winter)
(7, 2, 7, 7, 60);  -- Mohammed Ali in Collaborative Problem Solving (2026 Spring)

--Insertion into Learning_goal 
INSERT INTO Learning_goal ( status, deadline, description)
VALUES
('In Progress', '2024-12-15 23:59:59', 'Complete Python Basics course'),
('Completed', '2024-10-30 23:59:59', 'Finish Graphic Design Theory course'),
('In Progress', '2025-01-20 23:59:59', 'Learn Data Analysis Techniques'),
('Completed', '2024-11-05 23:59:59', 'Master Public Speaking Skills'),
('In Progress', '2025-03-01 23:59:59', 'Finish Creative Writing Workshop'),
('In Progress', '2025-06-01 23:59:59', 'Complete Leadership Fundamentals course'),
('Completed', '2025-08-15 23:59:59', 'Finish Collaborative Problem Solving course');
INSERT INTO Learning_goal ( status, deadline, description)
VALUES('In Progress', '2024-11-22 23:59:59', 'Complete Python Basics course');

--Insertion into LearnersGoals 
INSERT INTO LearnersGoals (GoalID, LearnerID)
VALUES
(1, 1),  -- Alice Smith with Learning Goal 1
(2, 2),  -- Mohammed Ali with Learning Goal 2
(3, 3),  -- Wei Zhang with Learning Goal 3
(4, 4),  -- Maria Gonzalez with Learning Goal 4
(5, 5),  -- John Doe with Learning Goal 5
(6, 1),  -- Alice Smith with Learning Goal 6
(7, 2);  -- Mohammed Ali with Learning Goal 7
INSERT INTO LearnersGoals (GoalID, LearnerID)
VALUES(8,1)

--Insertion into Survey 
INSERT INTO Survey ( Title)
VALUES
('Course Feedback Survey'),
('Learning Experience Survey'),
('Programming Skills Survey'),
('Design Knowledge Survey'),
('Public Speaking Feedback'),
('Creative Writing Feedback'),
('Leadership Skills Survey');

--Insertion into SurveyQuestions 
INSERT INTO SurveyQuestions (SurveyID, Question)
VALUES
(1, 'How satisfied are you with the course content?'),
(2, 'Did the learning experience meet your expectations?'),
(3, 'How comfortable were you with programming concepts?'),
(4, 'How well did the design principles resonate with you?'),
(5, 'Did the public speaking course help you improve your speaking skills?'),
(6, 'How useful did you find the creative writing exercises?'),
(7, 'Do you feel more confident in leadership after this course?');

--Insertion into FilledSurvey 
INSERT INTO FilledSurvey (SurveyID, Question, LearnerID, Answer)
VALUES
(1, 'How satisfied are you with the course content?', 1, 'Very Satisfied'),
(2, 'Did the learning experience meet your expectations?', 2, 'Yes'),
(3, 'How comfortable were you with programming concepts?', 3, 'Comfortable'),
(4, 'How well did the design principles resonate with you?', 4, 'Strongly Agree'),
(5, 'Did the public speaking course help you improve your speaking skills?', 5, 'Yes, a lot'),
(6, 'How useful did you find the creative writing exercises?', 1, 'Very Useful'),
(7, 'Do you feel more confident in leadership after this course?', 2, 'Yes');

--Insertion into Notification 
INSERT INTO Notification (timestamp, message, urgency_level)
VALUES
('2024-11-15 10:00:00', 'Your course content has been updated.', 'High'),
('2024-11-15 11:00:00', 'Reminder: Complete your survey by tomorrow.', 'Medium'),
('2024-11-15 12:00:00', 'Your progress in the course is at 80%. Keep going!', 'Low'),
('2024-11-15 13:00:00', 'New learning materials have been added.', 'High'),
('2024-11-15 14:00:00', 'Your participation in the survey is appreciated.', 'Low'),
('2024-11-15 15:00:00', 'Upcoming course session: Programming Basics', 'Medium'),
('2024-11-15 16:00:00', 'Your leadership goal is almost achieved! Keep it up.', 'High');

--Insertion into ReceivedNotification 
INSERT INTO ReceivedNotification (NotificationID, LearnerID)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 1),
(7, 2);

--Insertion into Badge 
INSERT INTO Badge ( title, description, criteria, points)
VALUES
( 'Programming Beginner', 'Awarded for completing the beginner programming course.', 'Complete Introduction to Programming', 10),
( 'Data Analysis Expert', 'Awarded for mastering advanced data analysis techniques.', 'Complete Advanced Data Analysis with top score', 20),
('Design Enthusiast', 'Awarded for demonstrating strong design skills.', 'Complete Graphic Design Basics with high scores', 15),
( 'Public Speaking Pro', 'Awarded for delivering excellent public speaking performances.', 'Complete Public Speaking course with positive feedback', 12),
('Creative Writer', 'Awarded for demonstrating creativity in writing tasks.', 'Complete Creative Writing Workshop with top grades', 18),
('Leadership Achiever', 'Awarded for outstanding leadership performance.', 'Complete Leadership Fundamentals with high evaluations', 25),
( 'Collaborative Problem Solver', 'Awarded for outstanding teamwork in solving problems.', 'Participate in Collaborative Problem Solving with successful team outcome', 22);

--Insertion into SkillProgression 
INSERT INTO SkillProgression ( proficiency_level, LearnerID, skill_name, timestamp)
VALUES
('Beginner', 1, 'Programming', '2024-11-15 10:00:00'),
('Intermediate', 1, 'Problem Solving', '2024-11-15 10:30:00'),
('Advanced', 2, 'Graphic Design', '2024-11-15 11:00:00'),
('Beginner', 2, 'Public Speaking', '2024-11-15 11:30:00'),
('Expert', 3, 'Data Analysis', '2024-11-15 12:00:00'),
('Intermediate', 4, 'Leadership', '2024-11-15 12:30:00'),
('Advanced', 5, 'Creative Writing', '2024-11-15 13:00:00');

--Insertion into Achievement 
INSERT INTO Achievement (LearnerID, BadgeID, description, date_earned, type)
VALUES
( 1, 1, 'Completed Introduction to Programming', '2024-11-15 10:30:00', 'Skill'),
( 2, 2, 'Mastered Advanced Data Analysis Techniques', '2024-11-15 11:30:00', 'Skill'),
( 3, 3, 'Demonstrated Proficiency in Graphic Design', '2024-11-15 12:30:00', 'Skill'),
( 4, 4, 'Delivered Outstanding Public Speaking Presentation', '2024-11-15 13:30:00', 'Skill'),
( 5, 5, 'Completed Creative Writing Workshop with Excellence', '2024-11-15 14:30:00', 'Skill'),
( 1, 6, 'Completed Leadership Fundamentals with High Evaluation', '2024-11-15 15:30:00', 'Leadership'),
( 2, 7, 'Successfully Solved Collaborative Problem-Solving Challenges', '2024-11-15 16:30:00', 'Teamwork');

-- Insertion into Reward
INSERT INTO Reward ( value, description, type)
VALUES
(50.00, 'Completion of introductory course', 'Points'),
(100.00, 'Completion of advanced course', 'Points'),
(20.00, 'Early submission of assignment', 'Bonus'),
( 75.00, 'Outstanding project performance', 'Points'),
(150.00, 'Completion of a difficult course', 'Bonus'),
(30.00, 'Contribution to peer learning', 'Points'),
(200.00, 'Winning a competition', 'Prize');

-- Insertion into Quest
INSERT INTO Quest ( difficulty_level, criteria, description, title)
VALUES
('Easy', 'Complete a beginner course', 'Complete an introductory programming course', 'Beginner Quest'),
( 'Medium', 'Complete 3 intermediate projects', 'Build 3 intermediate-level projects in design', 'Intermediate Quest'),
( 'Hard', 'Complete a data science certification', 'Complete a certification in Data Science and Analytics', 'Advanced Quest'),
( 'Medium', 'Collaborate with a team', 'Participate in a group project and contribute equally', 'Teamwork Quest'),
( 'Easy', 'Submit assignments on time', 'Submit all assignments before the deadline', 'Punctuality Quest'),
( 'Hard', 'Achieve top performance in a course', 'Earn an A in the course by demonstrating expertise', 'Excellence Quest'),
('Medium', 'Complete a leadership course', 'Finish a leadership development program', 'Leadership Quest');

-- Insertion into Skill_Mastery
INSERT INTO Skill_Mastery (QuestID, skill)
VALUES
(1, 'Programming'),
(2, 'Graphic Design'),
(3, 'Data Analysis'),
(4, 'Leadership'),
(5, 'Time Management'),
(6, 'Public Speaking'),
(7, 'Creative Writing');

-- Insertion into Collaborative
INSERT INTO Collaborative (QuestID, deadline, max_num_participants)
VALUES
(1, '2024-12-31 23:59:59', 10),
(2, '2025-01-15 23:59:59', 8),
(3, '2025-03-01 23:59:59', 5),
(4, '2025-04-10 23:59:59', 6),
(5, '2024-11-30 23:59:59', 15),
(6, '2025-06-01 23:59:59', 12),
(7, '2025-07-01 23:59:59', 7);
 
 -- Insertion into LearnersCollaboration
INSERT INTO LearnersCollaboration (LearnerID, QuestID, completion_status)
VALUES
(1, 1, 'Completed'),
(2, 2, 'In Progress'),
(3, 3, 'Not Started'),
(4, 4, 'Completed'),
(5, 5, 'In Progress'),
(1,5,'In progress');


INSERT INTO LearnerMastery (LearnerID, QuestID, skill, completion_status)
VALUES
(1, 1, 'Programming', 'Completed'),
(2, 2, 'Graphic Design', 'In Progress'),
(3, 3, 'Data Analysis', 'Not Started'),
(4, 4, 'Leadership', 'Completed'),
(5, 5, 'Time Management', 'In Progress');


-- Insertion into Discussion_forum
INSERT INTO Discussion_forum ( ModuleID, CourseID, title, last_active, timestamp, description)
VALUES
(1, 1, 'Discussion on Variables', '2024-11-10 15:00:00', '2024-11-01 09:00:00', 'Forum for learners to discuss and clarify the concept of variables in programming.'),
(1, 1, 'Discussion on Control Structures', '2024-11-12 17:30:00', '2024-11-02 10:00:00', 'Forum for discussing different control structures in programming and their uses.'),
(1, 1, 'Discussion on Functions and Loops', '2024-11-08 13:45:00', '2024-11-03 11:30:00', 'Forum for learners to discuss functions, loops, and their applications in coding.'),
(2, 1, 'Data Cleaning Techniques Discussion', '2024-11-13 09:00:00', '2024-11-04 14:00:00', 'Forum for discussing various data cleaning techniques used in data analysis.'),
(2, 1, 'Exploratory Data Analysis Insights', '2024-11-11 14:30:00', '2024-11-05 12:00:00', 'Forum for learners to share insights and best practices in exploratory data analysis.'),
(7, 3, 'Graphic Design Principles Discussion', '2024-11-09 16:00:00', '2024-11-06 12:30:00', 'Forum for discussing the principles of design, including layout, color theory, and typography.'),
(9, 4, 'Public Speaking Techniques Discussion', '2024-11-14 18:30:00', '2024-11-07 15:45:00', 'Forum for discussing effective public speaking techniques and tips to improve delivery.');

-- Insertion into LearnerDiscussion
INSERT INTO LearnerDiscussion (ForumID, LearnerID, Post, time)
VALUES
(1, 1, 'What are the best practices for initializing variables in different programming languages?', '2024-11-01 09:15:00'),
(2, 1, 'Can anyone explain the difference between if-else and switch-case in control structures?', '2024-11-02 10:30:00'),
(3, 2, 'I find using loops to iterate over lists a bit tricky. Can anyone provide examples?', '2024-11-03 11:45:00'),
(4, 3, 'What are some challenges you’ve encountered when cleaning real-world datasets?', '2024-11-04 14:30:00'),
(5, 4, 'How do you decide which visualization method is best during exploratory data analysis?', '2024-11-05 12:15:00'),
(6, 4, 'Does anyone have tips on how to make a design more user-friendly and accessible?', '2024-11-06 12:45:00'),
(7, 5, 'I need some tips on how to reduce nervousness during public speaking. Any advice?', '2024-11-07 16:00:00');

-- Insertion into QuestReward
INSERT INTO QuestReward (RewardID, QuestID, LearnerID, Time_earned)
VALUES
(1, 1, 1, '2024-11-01 10:00:00'),
(2, 1, 2, '2024-11-02 11:30:00'),
(3, 2, 3, '2024-11-03 12:45:00'),
(4, 3, 4, '2024-11-04 14:00:00'),
(5, 4, 5, '2024-11-05 15:15:00'),
(6, 5, 3, '2024-11-06 16:30:00'),
(7, 6, 1, '2024-11-07 17:45:00');

insert into takesassesment
values(1,2,96),
(3,2,100),
(1,5,88),
(2,4,99),
(4,1,22)

insert into takesassesment
values(7,1,96),
(7,2,20)




