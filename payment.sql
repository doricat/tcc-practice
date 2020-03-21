create table accounts
(
    id      bigint         not null primary key,
    user_id bigint         not null unique,
    balance decimal(10, 2) not null default 0
);

create table bills
(
    id             bigint         not null primary key,
    account_id     bigint         not null,
    amount         decimal(10, 2) not null,
    transaction_id bigint         not null,
    state          varchar(20)    not null,
    created_at     timestamp      not null,
    expires        timestamp      not null,
    updated_at     timestamp      not null,
    foreign key (account_id) references accounts (id)
);

create or replace function create_bill(_id bigint,
                                       _user_id bigint,
                                       _amount decimal(10, 2),
                                       _transaction_id bigint,
                                       _created_at timestamp,
                                       _expires timestamp)
    returns int /*0=Ok; 1=InsufficientBalance*/
as
$func$
declare
    __balance    decimal(10, 2);
    __account_id bigint;
begin
    select balance, id into __balance, __account_id from accounts where user_id = _user_id for update;
    if __balance + _amount < 0 then
        return 1;
    end if;

    update accounts set balance = balance + _amount where user_id = _user_id;
    insert into bills(id, account_id, amount, transaction_id, state, created_at, expires, updated_at)
    values (_id, __account_id, _amount, _transaction_id, 'Pending', _created_at, _expires, _created_at);

    return 0;
end;
$func$ language plpgsql;

create or replace function confirm_bill(_transaction_id bigint, _updated_at timestamp)
    returns bool
as
$func$
declare
    __state   varchar(20);
    __expires timestamp;
begin
    select state, expires into __state, __expires from bills where transaction_id = _transaction_id for update;
    if __state <> 'Pending' or __expires < _updated_at then
        return false;
    end if;

    update bills set state = 'Confirmed', updated_at = _updated_at where transaction_id = _transaction_id;
    return true;
end;
$func$ language plpgsql;

create or replace function cancel_bill(_transaction_id bigint, _updated_at timestamp)
    returns bool
as
$func$
declare
    __account_id bigint;
    __amount     decimal(10, 2);
    __state      varchar(20);
    __expires    timestamp;
begin
    select account_id, amount, state, expires
    into __account_id, __amount, __state, __expires
    from bills
    where transaction_id = _transaction_id for update;

    if __state <> 'Pending' or __expires < _updated_at then
        return false;
    end if;

    update bills set state = 'Canceled', updated_at = _updated_at where transaction_id = _transaction_id;
    update accounts set balance = balance + abs(__amount) where id = __account_id;
    return true;
end;
$func$ language plpgsql;

create or replace function notify_bill_state()
    returns trigger
as
$func$
declare
    __json text;
begin
    if (tg_op = 'INSERT') then
        __json = json_build_object(
                         'id', new.transaction_id,
                         'state', new.state,
                         'beginTime', new.created_at,
                         'expires', new.expires
                     ) #>> '{}';
    elseif (tg_op = 'UPDATE') then
        __json = json_build_object(
                         'id', old.transaction_id,
                         'state', new.state,
                         'beginTime', old.created_at,
                         'expires', old.expires
                     ) #>> '{}';
    end if;

    if (__json is not null) then
        perform pg_notify('transaction_channel', __json);
    end if;

    return null;
end;
$func$ language plpgsql;

create trigger state_trigger
    after insert or update
    on bills
    for each row
execute function notify_bill_state();

insert into accounts(id, user_id, balance) values (11704070658064300, 11704070658064385, 1000);