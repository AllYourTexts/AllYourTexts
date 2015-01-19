CREATE TABLE msg_pieces (ROWID INTEGER PRIMARY KEY AUTOINCREMENT, message_id INTEGER, data BLOB, part_id INTEGER, preview_part INTEGER, content_type TEXT, height INTEGER, version INTEGER, flags INTEGER, content_id TEXT, content_loc TEXT, headers BLOB);
CREATE TABLE msg_group (ROWID INTEGER PRIMARY KEY AUTOINCREMENT, type INTEGER, newest_message INTEGER, unread_count INTEGER, hash INTEGER);
CREATE TABLE message (ROWID INTEGER PRIMARY KEY AUTOINCREMENT, address TEXT, date INTEGER, text TEXT, flags INTEGER, replace INTEGER, svc_center TEXT, group_id INTEGER, association_id INTEGER, height INTEGER, UIFlags INTEGER, version INTEGER, subject TEXT, country TEXT, headers BLOB, recipients BLOB, read INTEGER);
CREATE TABLE group_member (ROWID INTEGER PRIMARY KEY AUTOINCREMENT, group_id INTEGER, address TEXT, country TEXT);
CREATE TABLE _SqliteDatabaseProperties (key TEXT, value TEXT, UNIQUE(key));
