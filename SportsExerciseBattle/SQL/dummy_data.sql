-- Insert dummy users
INSERT INTO Users (username, password, name, bio, elo) VALUES
('john_doe', '$2a$10$rNCLVJQLLzUf3hNSkf0MUeM5JS0JWv5AS8y2LZbwSJ1BJH5BQsXZO', 'John Doe', 'This is John Doe''s bio.', 1750),
('jane_smith', '$2a$10$rNCLVJQLLzUf3hNSkf0MUeM5JS0JWv5AS8y2LZbwSJ1BJH5BQsXZO', 'Jane Smith', 'This is Jane Smith''s bio.', 1900),
('mike_jones', '$2a$10$rNCLVJQLLzUf3hNSkf0MUeM5JS0JWv5AS8y2LZbwSJ1BJH5BQsXZO', 'Mike Jones', 'This is Mike Jones''s bio.', 1650),
('sarah_wilson', '$2a$10$rNCLVJQLLzUf3hNSkf0MUeM5JS0JWv5AS8y2LZbwSJ1BJH5BQsXZO', 'Sarah Wilson', 'This is Sarah Wilson''s bio.', 1800),
('david_brown', '$2a$10$rNCLVJQLLzUf3hNSkf0MUeM5JS0JWv5AS8y2LZbwSJ1BJH5BQsXZO', 'David Brown', 'This is David Brown''s bio.', 1500);

-- Insert tournaments
INSERT INTO Tournaments (start_time, end_time, status) VALUES
(CURRENT_TIMESTAMP - INTERVAL '1 day', CURRENT_TIMESTAMP + INTERVAL '6 days', 'RUNNING'),
(CURRENT_TIMESTAMP - INTERVAL '10 days', CURRENT_TIMESTAMP - INTERVAL '3 days', 'ENDED'),
(CURRENT_TIMESTAMP + INTERVAL '3 days', CURRENT_TIMESTAMP + INTERVAL '10 days', 'ENDED');

-- Register users for tournaments
-- Active tournament (id=1)
INSERT INTO TournamentParticipants (tournament_id, username) VALUES
(1, 'john_doe'),
(1, 'jane_smith'),
(1, 'mike_jones'),
(1, 'sarah_wilson');

-- Completed tournament (id=2)
INSERT INTO TournamentParticipants (tournament_id, username) VALUES
(2, 'john_doe'),
(2, 'jane_smith'),
(2, 'david_brown');

-- Upcoming tournament (id=3)
INSERT INTO TournamentParticipants (tournament_id, username) VALUES
(3, 'jane_smith'),
(3, 'mike_jones'),
(3, 'david_brown'),
(3, 'sarah_wilson');

-- Insert exercise entries for active tournament
INSERT INTO ExerciseEntries (username, name, count, duration_in_seconds, timestamp, tournament_id) VALUES
-- John's entries for active tournament
('john_doe', 'Push-ups', 25, 120, CURRENT_TIMESTAMP - INTERVAL '20 hours', 1),
('john_doe', 'Sit-ups', 30, 180, CURRENT_TIMESTAMP - INTERVAL '12 hours', 1),
-- Jane's entries for active tournament
('jane_smith', 'Pull-ups', 15, 90, CURRENT_TIMESTAMP - INTERVAL '18 hours', 1),
('jane_smith', 'Squats', 40, 210, CURRENT_TIMESTAMP - INTERVAL '8 hours', 1),
('jane_smith', 'Burpees', 20, 150, CURRENT_TIMESTAMP - INTERVAL '2 hours', 1),
-- Mike's entries for active tournament
('mike_jones', 'Push-ups', 20, 100, CURRENT_TIMESTAMP - INTERVAL '15 hours', 1),
-- Sarah's entries for active tournament
('sarah_wilson', 'Sit-ups', 35, 200, CURRENT_TIMESTAMP - INTERVAL '10 hours', 1),
('sarah_wilson', 'Pull-ups', 12, 80, CURRENT_TIMESTAMP - INTERVAL '5 hours', 1);

-- Insert exercise entries for completed tournament
INSERT INTO ExerciseEntries (username, name, count, duration_in_seconds, timestamp, tournament_id) VALUES
-- John's entries for completed tournament
('john_doe', 'Push-ups', 22, 110, CURRENT_TIMESTAMP - INTERVAL '9 days', 2),
('john_doe', 'Sit-ups', 28, 170, CURRENT_TIMESTAMP - INTERVAL '8 days', 2),
('john_doe', 'Pull-ups', 10, 60, CURRENT_TIMESTAMP - INTERVAL '7 days', 2),
('john_doe', 'Squats', 35, 190, CURRENT_TIMESTAMP - INTERVAL '6 days', 2),
-- Jane's entries for completed tournament
('jane_smith', 'Push-ups', 30, 150, CURRENT_TIMESTAMP - INTERVAL '9 days', 2),
('jane_smith', 'Burpees', 25, 170, CURRENT_TIMESTAMP - INTERVAL '7 days', 2),
-- David's entries for completed tournament
('david_brown', 'Sit-ups', 20, 120, CURRENT_TIMESTAMP - INTERVAL '8 days', 2),
('david_brown', 'Push-ups', 15, 90, CURRENT_TIMESTAMP - INTERVAL '6 days', 2),
('david_brown', 'Squats', 25, 150, CURRENT_TIMESTAMP - INTERVAL '5 days', 2);