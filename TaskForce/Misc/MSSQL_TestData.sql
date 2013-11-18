use TaskForce

truncate table Account
truncate table Filter
truncate table FilterFilterGroup
truncate table FilterGroup
truncate table FilterStatus
truncate table Protocol

insert into Account (LoginName, LastLogin) values ('Administrator', GETDATE())

exec InsFilterGroup 0, 'Group1'

exec InsFilter 0, 'Admin1Filter1', 1, 'Admin1.exe', 0

exec LstProtectedFilter 0
exec LstForbiddenFilter 0

-------------------------------------------------------------------------------------------
exec InsAccount 'Domain\Pupil1'
exec InsFilterGroup 1, 'GroupA'
exec InsFilterGroup 1, 'GroupB'

exec InsFilter 1, 'Pupil1Filter1', 1, 'Pupil1A.exe', 2
exec InsFilter 1, 'Pupil1Filter1', 1, 'Pupil1A.exe', 2

exec LstProtectedFilter 1
exec LstForbiddenFilter 1

exec InsProtocol 1, 1, '10.0.0.0.1'
exec InsProtocol 1, 2, '10.0.0.0.1'

exec LstProtocol 1

-------------------------------------------------------------------------------------------
exec InsAccount 'Domain\Pupil2'
exec LstProtectedFilter 2
exec LstForbiddenFilter 2

exec InsFilter 2, 'Pupil12', 1, 'Pupil2A.exe', 2
exec InsFilter 2, 'Pupil12', 1, 'Pupil2A.exe', 2

exec InsProtocol 2, 1, '10.0.0.0.1'
exec InsProtocol 2, 5, '10.0.0.0.1'

exec LstProtocol 2

-------------------------------------------------------------------------------------------