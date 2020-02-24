create database product_db;

create table products
(
    id          bigint         not null primary key,
    name        varchar(200)   not null unique,
    price       decimal(10, 2) not null,
    qty         int            not null,
    description text           null
);