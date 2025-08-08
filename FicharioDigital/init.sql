CREATE EXTENSION IF NOT EXISTS unaccent;

CREATE INDEX idx_patients_name_unaccent
    ON patients (unaccent(name) text_pattern_ops);