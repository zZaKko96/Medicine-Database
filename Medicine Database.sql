--
-- PostgreSQL database dump
--

\restrict TwgUb8P6rtf67eq66fOZZk5pzQ2BI3TJxhXsyzXo7rMhDHgoxylLNMPbdrtfzCb

-- Dumped from database version 17.6
-- Dumped by pg_dump version 17.6

-- Started on 2025-09-15 19:42:05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 16398)
-- Name: appointment; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.appointment (
    id integer NOT NULL,
    data date,
    diagnosis character varying(50),
    patient_id integer NOT NULL,
    doctor_id integer NOT NULL,
    hour time(6) without time zone
);


ALTER TABLE public.appointment OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16393)
-- Name: doctor; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.doctor (
    id integer NOT NULL,
    name character varying(20),
    surname character varying(20),
    specialization character varying(100),
    hospital_id integer NOT NULL
);


ALTER TABLE public.doctor OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16403)
-- Name: hospital; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hospital (
    id integer NOT NULL,
    name character varying(40),
    address character varying(100)
);


ALTER TABLE public.hospital OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16388)
-- Name: patient; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.patient (
    id integer NOT NULL,
    name character varying(20),
    surname character varying(20),
    "day of birth" date,
    phone character varying(10)
);


ALTER TABLE public.patient OWNER TO postgres;

--
-- TOC entry 4914 (class 0 OID 16398)
-- Dependencies: 219
-- Data for Name: appointment; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.appointment (id, data, diagnosis, patient_id, doctor_id, hour) FROM stdin;
1	2025-09-08	Здоровий/а	1	1	16:00:00
2	2025-09-08	Здоровий/а	2	1	18:00:00
3	2025-09-09	Здоровий/а	3	2	20:30:00
\.


--
-- TOC entry 4913 (class 0 OID 16393)
-- Dependencies: 218
-- Data for Name: doctor; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.doctor (id, name, surname, specialization, hospital_id) FROM stdin;
1	Володимир	Пецентій	серцево-судинний хірург	2
2	Олександр	Зінчук	проктолог	2
3	Леся	Цибульська	терапевт	1
\.


--
-- TOC entry 4915 (class 0 OID 16403)
-- Dependencies: 220
-- Data for Name: hospital; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.hospital (id, name, address) FROM stdin;
1	Луцька районна лікарня	100, вулиця Теремнівська, Липини
2	Волинська обласна клінічна лікарня	проспект Президента Грушевського, 21, Луцьк
\.


--
-- TOC entry 4912 (class 0 OID 16388)
-- Dependencies: 217
-- Data for Name: patient; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.patient (id, name, surname, "day of birth", phone) FROM stdin;
1	Захар	Косарук	2006-03-04	0956033234
2	Аміна	Ватраль	2004-04-23	0979544653
3	Сергій	Косарук	1964-03-05	0661436232
\.


--
-- TOC entry 4758 (class 2606 OID 16402)
-- Name: appointment appointment_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointment
    ADD CONSTRAINT appointment_pkey PRIMARY KEY (id);


--
-- TOC entry 4756 (class 2606 OID 16397)
-- Name: doctor doctor_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.doctor
    ADD CONSTRAINT doctor_pkey PRIMARY KEY (id);


--
-- TOC entry 4760 (class 2606 OID 16407)
-- Name: hospital hospital_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hospital
    ADD CONSTRAINT hospital_pkey PRIMARY KEY (id);


--
-- TOC entry 4754 (class 2606 OID 16392)
-- Name: patient patient_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.patient
    ADD CONSTRAINT patient_pkey PRIMARY KEY (id);


--
-- TOC entry 4763 (class 2606 OID 16418)
-- Name: appointment appointment_doctor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointment
    ADD CONSTRAINT appointment_doctor_id_fkey FOREIGN KEY (doctor_id) REFERENCES public.doctor(id) NOT VALID;


--
-- TOC entry 4764 (class 2606 OID 16433)
-- Name: appointment appointment_doctor_id_fkey1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointment
    ADD CONSTRAINT appointment_doctor_id_fkey1 FOREIGN KEY (doctor_id) REFERENCES public.doctor(id) NOT VALID;


--
-- TOC entry 4765 (class 2606 OID 16413)
-- Name: appointment appointment_patient_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointment
    ADD CONSTRAINT appointment_patient_id_fkey FOREIGN KEY (patient_id) REFERENCES public.patient(id) NOT VALID;


--
-- TOC entry 4766 (class 2606 OID 16428)
-- Name: appointment appointment_patient_id_fkey1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointment
    ADD CONSTRAINT appointment_patient_id_fkey1 FOREIGN KEY (patient_id) REFERENCES public.patient(id) NOT VALID;


--
-- TOC entry 4761 (class 2606 OID 16408)
-- Name: doctor doctor_hospital_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.doctor
    ADD CONSTRAINT doctor_hospital_id_fkey FOREIGN KEY (hospital_id) REFERENCES public.hospital(id) NOT VALID;


--
-- TOC entry 4762 (class 2606 OID 16423)
-- Name: doctor doctor_hospital_id_fkey1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.doctor
    ADD CONSTRAINT doctor_hospital_id_fkey1 FOREIGN KEY (hospital_id) REFERENCES public.hospital(id) NOT VALID;


-- Completed on 2025-09-15 19:42:05

--
-- PostgreSQL database dump complete
--

\unrestrict TwgUb8P6rtf67eq66fOZZk5pzQ2BI3TJxhXsyzXo7rMhDHgoxylLNMPbdrtfzCb

