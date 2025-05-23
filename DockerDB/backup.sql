PGDMP                      }            AtonDB    17.2 (Debian 17.2-1.pgdg120+1)    17.2 (Debian 17.2-1.pgdg120+1) 
    &           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            '           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            (           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            )           1262    16384    AtonDB    DATABASE     s   CREATE DATABASE "AtonDB" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';
    DROP DATABASE "AtonDB";
                     maxim    false            �            1259    16394    Users    TABLE     �  CREATE TABLE public."Users" (
    id uuid NOT NULL,
    "Login" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Name" text NOT NULL,
    "Gender" integer NOT NULL,
    "Birthday" timestamp with time zone,
    "Admin" boolean NOT NULL,
    "CreateOn" timestamp with time zone,
    "CreatedBy" text NOT NULL,
    "ModofiedOn" timestamp with time zone,
    "ModifiedBy" text NOT NULL,
    "RevokerOn" timestamp with time zone,
    "RevokedBy" text NOT NULL
);
    DROP TABLE public."Users";
       public         heap r       maxim    false            �            1259    16389    __EFMigrationsHistory    TABLE     �   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap r       maxim    false            #          0    16394    Users 
   TABLE DATA           �   COPY public."Users" (id, "Login", "PasswordHash", "Name", "Gender", "Birthday", "Admin", "CreateOn", "CreatedBy", "ModofiedOn", "ModifiedBy", "RevokerOn", "RevokedBy") FROM stdin;
    public               maxim    false    218            "          0    16389    __EFMigrationsHistory 
   TABLE DATA           R   COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
    public               maxim    false    217            �           2606    16400    Users PK_Users 
   CONSTRAINT     P   ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY (id);
 <   ALTER TABLE ONLY public."Users" DROP CONSTRAINT "PK_Users";
       public                 maxim    false    218            �           2606    16393 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public                 maxim    false    217            #   �  x�u�Os�0���S��[GF+���o-
\��`����'�����5tȤ!�i潕�OoS��)Y�aHI�b�x��3�!ѹ��]^mZ^�v�,�����0|���V�����ճB۽��p?N՗Y��S�r�#�P�+�υ/�k�>s��oEDWjAZ��K���|��+�ͱ�Y�!�y�'��D��L�B�jDa�h�/�p�h4Qz0m&�EՋ�Z��Y��<V��>�:Mw�g4v�E�z���gx��}��n��C�H����+�[K=/}�Q�w�\�y����nR�CQ�E4�^E�����t*&�o�`�G[|�v��~w�n��1ۆ�?7��V^����:)�߇�TT�n_��w5i_��%qKw�\��|���I���$������a1���LdS�8�C]9�Ѣ\M�e���*�hn�v�M�X
�A���縄��l�Tۨ��6��sd�8�� kMh��m�q�p�W��n�r��]��q      "   0   x�32025054724622����,�L�q.JM,I��3�3����� �3	�     