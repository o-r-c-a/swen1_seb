CREATE TABLE IF NOT EXISTS Users (
    username VARCHAR(50) PRIMARY KEY,
    password VARCHAR(255) NOT NULL,
    authtoken VARCHAR(255),
    name VARCHAR(100),
    bio TEXT,
    image TEXT,
    elo INTEGER
);

CREATE TABLE IF NOT EXISTS Tournaments (
    id SERIAL PRIMARY KEY,
    start_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    end_time TIMESTAMP NOT NULL,
    status VARCHAR(20) NOT NULL
);

CREATE TABLE IF NOT EXISTS ExerciseEntries (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    name VARCHAR(50) NOT NULL,
    count INTEGER NOT NULL,
    duration_in_seconds INTEGER NOT NULL,
    timestamp TIMESTAMPTZ NOT NULL,
    tournament_id INTEGER,
    FOREIGN KEY (username) REFERENCES Users(username) ON DELETE CASCADE,
	FOREIGN KEY (tournament_id) REFERENCES Tournaments(id) ON DELETE SET NULL
);


CREATE TABLE IF NOT EXISTS TournamentParticipants (
    tournament_id INTEGER NOT NULL,
    username VARCHAR(50) NOT NULL,
    PRIMARY KEY (tournament_id, username),
    FOREIGN KEY (tournament_id) REFERENCES Tournaments(id) ON DELETE CASCADE,
    FOREIGN KEY (username) REFERENCES Users(username) ON DELETE CASCADE
);