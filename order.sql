create table orders
(
    id             bigint      not null primary key,
    user_id        bigint      not null,
    transaction_id bigint      not null,
    state          varchar(20) not null,
    created_at     timestamp   not null,
    expires        timestamp   not null,
    updated_at     timestamp   not null
);

create table order_items
(
    id         bigint         not null primary key,
    order_id   bigint         not null,
    product_id bigint         not null,
    price      decimal(10, 2) not null,
    qty        int            not null,
    foreign key (order_id) references orders (id)
);

create or replace function confirm_order(_transaction_id bigint, _updated_at timestamp)
    returns bool
as
$func$
declare
    __state   varchar(20);
    __expires timestamp;
begin
    select state, expires into __state, __expires from orders where transaction_id = _transaction_id for update;

    if __state <> 'Pending' or __expires < _updated_at then
        return false;
    end if;

    update orders set state = 'Confirmed', updated_at = _updated_at where transaction_id = _transaction_id;
    return true;
end;
$func$ language plpgsql;

create or replace function cancel_order(_transaction_id bigint, _updated_at timestamp)
    returns bool
as
$func$
declare
    __state      varchar(20);
    __expires    timestamp;
begin
    select state, expires
    into __state, __expires
    from orders
    where transaction_id = _transaction_id for update;

    if __state <> 'Pending' or __expires < _updated_at then
        return false;
    end if;

    update orders set state = 'Canceled', updated_at = _updated_at where transaction_id = _transaction_id;

    return true;
end;
$func$ language plpgsql;

create or replace function notify_order_state()
    returns trigger
as
$func$
begin
    perform pg_notify('transaction_channel', json_build_object(
                                                     'id', new.transaction_id,
                                                     'state', new.state,
                                                     'beginTime', new.created_at,
                                                     'expires', new.expires
                                                 ) #>> '{}');
    return new;
end;
$func$ language plpgsql;

create trigger state_trigger
    after insert or update
    on orders
    for each row
execute function notify_order_state();