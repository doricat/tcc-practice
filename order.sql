create database order_db;

create table orders
(
    id         bigint    not null primary key,
    user_id    bigint    not null,
    bill_id    bigint    not null,
    state      int       not null,
    created_at timestamp not null,
    expires    timestamp not null,
    updated_at timestamp not null
);

create table order_items
(
    id         bigint         not null primary key,
    order_id   bigint         not null,
    product_id bigint         not null,
    price      decimal(10, 2) not null,
    qty        int            not null,
    foreign key (product_id) references products (id),
    foreign key (order_id) references orders (id)
);

create table shopping_cars
(
    id         bigint not null primary key,
    user_id    bigint not null,
    product_id bigint not null,
    qty        int    not null,
    foreign key (product_id) references products (id)
);