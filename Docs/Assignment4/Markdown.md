# VIA Event Association - Documentation

## Requirements Overview

This document outlines the use cases for the VIA Event Association's event management system. It provides a detailed
view of each scenario including success and failure states, structured for clear understanding and tracking of
implementation progress.

### Actors

- **Anonymous**: A non-registered, non-logged-in user.
- **Guest**: A registered user who can sign up for events.
- **Creator**: Represents the VIA Event Association actions, not an actual system user.

### Use Cases

#### UC1 - Event Creator Creates New Event

- [✅] **User Story**: In order to host events, as a creator, I want to create a new event.
    - **Success Scenarios**:
        - [✅] S1: Given an ID, when creator selects to create an event, then an empty event is created with an ID, the
          status is set to "draft", and the maximum number of guests is 5.
        - [✅] S2: Given an ID, when creator selects to create an event, then the event is created, and the title of the
          event is set to "Working Title".
        - [✅] S3: Given an ID, when creator selects to create an event, then the event is created, and the description
          of the event is set to an empty text, i.e. "".
        - [✅] S4: Given an ID, when creator selects to create an event, then the event is created, and the visibility of
          the event is private.
    - **Failure Scenarios**:
        - [✅] (No failure scenarios specified for this use case.)

#### UC2 - Event Creator Updates Title of Event

- [✅] **User Story**: In order to catch the interest of guests, as a creator, I want to set the title of an event.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID, when creator selects to set the title of the event, then the title of
          the event is updated, provided the title is between 3 and 75 characters and the event is in draft or ready
          status.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID, when creator selects to set the title of the event, then a failure
          message is returned explaining the rules if the title is not between 3 and 75 characters.
        - [✅] F2: Given an existing event with ID, when creator selects to set the title of the event and the event is
          in active or cancelled status, then a failure message is returned explaining that modification is not allowed.

#### UC3 - Event Creator Updates Description of Event

- [✅] **User Story**: In order to inform guests about the content of an event, as a creator, I want to set the
  description of an event.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID and the status is draft, when creator selects to set the description
          of the event, then the description of the event is updated, provided the description is between 0 and 250
          characters.
        - [✅] S2: Given an existing event with ID, when creator selects to set the description of the event to
          nothing/empty, then the description of the event is set to an empty description (i.e. "").
        - [✅] S3: Given an existing event with ID and the status is ready, when creator selects to set the description
          of the event to a valid value, then the description of the event is updated and the event status is changed to
          draft.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID, when creator selects to set the description of the event and the
          description is more than 250 characters, then a failure message is returned explaining the problem.
        - [✅] F2: Given an existing event with ID, when creator selects to set the description of the event and the
          event is in cancelled status, then a failure message is returned.
        - [✅] F3: Given an existing event with ID, when creator selects to set the description of the event and the
          event is in active status, then a failure message is returned.

#### UC4 - Event Creator Updates Start and End Time of the Event

- [✅] **User Story**: In order to inform guests when to show up and when to leave, as a creator, I want to set the start
  time/date and end time/date of the event.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID and the event status is draft, when creator selects to set the times
          of the event, then the times of the event are updated, meeting all specified conditions.
        - [✅] S2: Given an existing event with ID and the event status is draft, when creator selects to set the times
          of the event, then the times of the event are updated, meeting all specified conditions for cross-date events.
        - [✅] S3: Given an existing event with ID and the event status is ready, when creator sets the times of the
          event to valid values, then the times of the event are updated and the status is draft.
        - [✅] S4: Given an existing event with ID, when creator sets the times of the event to valid values and the
          start time is in the future, then the times of the event are updated.
        - [✅] S5: Given an existing event with ID, when creator sets the times of the event to valid values and the
          duration from start to finish is 10 hours or less, then the times of the event are updated.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID, when creator selects to set the times of the event and the start date
          is after the end date, then a failure message is returned.
        - [✅] F2: Given an existing event with ID, when creator selects to set the times of the event and the start time
          is after the end time, then a failure message is returned.
        - [✅] F3: Given an existing event with ID, when creator selects to set the times of the event and the duration
          is less than 1 hour, then a failure message is returned.
        - [✅] F4: Given an existing event with ID, when creator selects to set the times of the event and the time
          interval is invalid, then a failure message is returned.
        - [✅] F5: Given an existing event with ID, when creator selects to set the times of the event and the start time
          is before 08:00, then a failure message is returned.
        - [✅] F6: Given an existing event with ID, when creator selects to set the times of the event and the start time
          is before 01:00, then a failure message is returned.
        - [✅] F7: Given an existing event with ID and the event status is active, when creator sets the times of the
          event, then a failure message is returned.
        - [✅] F8: Given an existing event with ID, when creator sets the times of the event and the event is in
          cancelled status, then a failure message is returned.
        - [✅] F9: Given an existing event with ID, when creator selects to set the times of the event and the duration
          of the event is longer than 10 hours, then a failure message is returned.
        - [✅] F10: Given an existing event with ID, when creator sets the times of the event and the start time is in
          the past, then a failure message is returned.
        - [✅] F11: Given an existing event with ID, when creator selects to set the times of the event and the time
          spans the restricted hours, then a failure message is returned.

#### UC5 - Event Creator Makes the Event Public

- [✅] **User Story**: In order to let any guest join the event, as a creator, I want to make the event public.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID and the status is draft, ready, or active, when creator chooses to
          make the event public, then the event is made public and the status is unchanged.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID and the event is in cancelled status, when creator chooses to make the
          event public, then a failure message is provided.

#### UC6 - Event Creator Makes the Event Private

- [✅] **User Story**: In order to only let invited guests join an event, as a creator, I want to make the event private.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID and the status is draft or ready, when creator chooses to make the
          event private, then the event is made private and the status is updated to draft.
        - [✅] S2: Given an existing event with ID and the status is draft or ready and the event is already public, when
          creator chooses to make the event private, then the event is made private.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID and the event is in active status, when creator chooses to make the
          event private, then a failure message is provided.
        - [✅] F2: Given an existing event with ID and the event is in cancelled status, when creator chooses to make the
          event private, then a failure message is provided.

#### UC7 - Event Creator Sets Maximum Number of Guests

- [✅] **User Story**: In order to not violate fire regulations, as a creator, I want to set the maximum number of
  guests.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID and the event status is draft or ready, when creator sets the maximum
          number of guests, then the maximum number of guests is set to the selected value, provided it is less than 50.
        - [✅] S2: Given an existing event with ID and the event status is draft or ready, when creator sets the maximum
          number of guests, then the maximum number of guests is set to the selected value, provided it is larger than
          or equal to 5.
        - [✅] S3: Given an existing event with ID and the event is in active status, when creator sets the maximum
          number of guests and the number is between 5 and 50 and not less than the previous value, then the maximum
          number of guests is set to the selected value.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID and the event is in active status, when creator reduces the number of
          maximum guests, then a failure message is provided.
        - [✅] F2: Given an existing event with ID and the event is in cancelled status, when creator sets the number of
          maximum guests, then a failure message is provided.
        - [✅] F3: Given an existing event with ID and the event has a location, when creator sets the maximum number of
          guests and the number is larger than the location’s capacity, then the request is rejected.
        - [✅] F4: Given an existing event with ID, when creator sets the number of maximum guests to a number less than
          5, then a failure message is provided.
        - [✅] F5: Given an existing event with ID, when creator sets the number of maximum guests to a number greater
          than 50, then a failure message is provided.

#### UC8 - Event Creator Readies an Event

- [✅] **User Story**: In order to finalize event setup, as a creator, I want to ready the event.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID and the event is in draft status and all required data is set with
          valid values, when creator readies the event, then the event is made ready.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID and the event is in draft status and any required data is not set with
          valid values, when creator readies the event, then a failure message is provided.
        - [✅] F2: Given an existing event with ID and the event is in cancelled status, when creator readies the event,
          then a failure message is provided.
        - [✅] F3: Given an existing event with ID and the event has a start date/time which is past, when the creator
          readies the event, then a failure message is provided.
        - [✅] F4: Given an existing event with ID and the title of the event is the default, when creator readies the
          event, then a failure message is provided.

#### UC9 - Event Creator Activates an Event

- [✅] **User Story**: In order to make the event available, as a creator, I want to activate the event.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID and the event is in draft status and all required data is set with
          valid values, when creator activates the event, then the event is first made ready and then made active.
        - [✅] S2: Given an existing event with ID and the event is in ready status, when creator activates the event,
          then the event is made active.
        - [✅] S3: Given an existing event with ID and the event is already in active status, when creator activates the
          event, then nothing changes, the event remains active.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID and the event is in draft status and any required data is not set with
          valid values, when creator activates the event, then a failure message is provided.
        - [✅] F2: Given an existing event with ID and the event is in cancelled status, when creator activates the
          event, then a failure message is provided.

#### UC10 - Anonymous Registers a New Account

- [✅] **User Story**: In order to properly use the platform, as an anonymous user, I want to register as a Guest.
    - **Success Scenarios**:
        - [✅] S1: Given via-email, first name, and last name and all other registration requirements are met, when Anon
          chooses to register, then a new account is created.
    - **Failure Scenarios**:
        - [✅] F1: Given email, when Anon chooses to register and the email does not end with "@via.dk", then the request
          is rejected.
        - [✅] F2: Given email, when Anon chooses to register and the email is not in correct format, then the request is
          rejected.
        - [✅] F3: Given first name, when Anon chooses to register and the first name is invalid, then the request is
          rejected.
        - [✅] F4: Given last name, when Anon chooses to register and the last name is invalid, then the request is
          rejected.
        - [✅] F5: Given email, when Anon chooses to register and the email is already registered, then the request is
          rejected.
        - [✅] F6: Given first name or last name, when Anon chooses to register and the name contains numbers, then the
          request is rejected.
        - [✅] F7: Given first name or last name, when Anon chooses to register and the name contains symbols, then the
          request is rejected.

#### UC11 - Guest Participates in Public Event

- [✅] **User Story**: In order to indicate my intention to participate in an event, as a guest, I want to choose to
  participate in a public event.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID, the event status is active, the event is public, a registered guest
          with ID, the current number of registered guests is less than the maximum number of allowed guests, and the
          event has not yet started, when the guest chooses to attend the public event, then the event has registered
          that the guest intends to participate.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing valid event with ID and the event status is not active, when guest chooses to
          participate in the event, then the request is rejected.
        - [✅] F2: Given an existing valid event with ID and the event status is active and the number of registered
          guests equals the maximum number of allowed guests, when guest chooses to participate in the event, then the
          request is rejected.
        - [✅] F3: Given an existing valid event with ID and the event start time is in the past, when guest chooses to
          participate in the event, then the request is rejected.
        - [✅] F4: Given an existing valid event with ID and the event is private, when guest chooses to participate in
          the event, then the request is rejected.
        - [✅] F5: Given an existing valid event with ID and the guest is already a participant, when guest chooses to
          participate in the event, then the request is rejected.

#### UC12 - Guest Cancels Event Participation

- [✅] **User Story**: In order to regret my intention to participate in an event, as a guest, I want to choose to cancel
  my participation intent in an event.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID and a registered guest with ID and the guest is currently marked as
          participating in the event, when the guest chooses to cancel their participation, then the event removes the
          participation of this guest.
        - [✅] S2: Given an existing event with ID and a registered guest with ID and the guest is not marked as
          participating in the event, when the guest chooses to cancel their participation, then nothing changes.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID and a registered guest with ID and the guest is marked as
          participating in the event and the event start time is in the past, when the guest chooses to cancel their
          participation, then the request is rejected.

#### UC13 - Guest is Invited to Event

- [✅] **User Story**: In order to nudge guests to participate in an event, as a creator, I want to invite guests to an
  event.
    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID, the event status is ready or active, and a registered guest with ID,
          when the creator invites a guest, then a pending guest invitation is registered on the event.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID, the event status is draft or cancelled, and a registered guest with
          ID, when the creator invites a guest, then the request is rejected.
        - [✅] F2: Given an existing event with ID, the event status is active, a registered guest with ID, and the
          maximum number of guests is already attending, when the creator invites a guest, then the request is rejected.

#### UC14 - Guest Accepts Invitation

- [✅] **User Story**: In order to join the event of which I was invited, as a guest, I want to accept the invitation.
    - **Success Scenarios**:
        - [✅] S1: Given an active event, a registered guest, and the event has a pending invitation for the guest, and
          the number of participating guests is less than the maximum number of guests, when the guest accepts the
          invitation, then the invitation is changed from pending to accepted.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID and a registered guest with ID and the event has no invitation for the
          guest, when the guest accepts an invitation, then the request is rejected.
        - [✅] F2: Given an existing event with ID and a registered guest with ID and the event has a pending invitation
          for the guest and the number of participating guests has reached the maximum, when the guest accepts the
          invitation, then the request is rejected.
        - [✅] F3: Given a cancelled event and a registered guest and the event has a pending invitation for the guest,
          when the guest accepts the invitation, then the request is rejected.
        - [✅] F4: Given a ready event and a registered guest and the event has a pending invitation for the guest, when
          the guest accepts the invitation, then the request is rejected.

#### UC15 - Guest Declines Invitation

- [✅] **User Story**: In order to indicate I will not be participating in an event to which I have been invited, as a
  guest, I want to decline an invitation.
    - **Success Scenarios**:
        - [✅] S1: Given an active event and a registered guest, and the event has an invitation for the guest, when the
          guest declines the invitation, then the invitation is registered as declined.
        - [✅] S2: Given an active event and a registered guest, and the event has an accepted invitation for the guest,
          when the guest declines the invitation, then the invitation is registered as declined.
    - **Failure Scenarios**:
        - [✅] F1: Given an existing event with ID and a registered guest, when the guest accepts the invitation, then
          the request is rejected, with a message explaining the guest is not invited to the event.
        - [✅] F2: Given a cancelled event and a registered guest, and the event has a pending invitation for the guest,
          when the guest declines the invitation, then the request is rejected with a message explaining invitations to
          cancelled events cannot be declined.
        - [✅] F3: Given a ready event and a registered guest, and the event has a pending invitation for the guest, when
          the guest accepts the invitation, then the request is rejected with a message explaining the event cannot yet
          be declined.

#### UC16 - Event Creator Adds New Location

- [✅] **User Story**: In order to manage event locations effectively, as an event creator, I want to add new locations.
    - **Success Scenarios**:
        - [✅] S1: When a creator adds a new location with valid details, then the location is created with a null ID,
          enters a draft status, and the maximum number of guests is set to 5.
        - [✅] S2: When a creator adds a new location, then the location should have the maximum number of guests set to
          5 by default.
        - [✅] S3: When a creator adds a new location, then the location should initially have an empty list of events.
    - **Failure Scenarios**:
        - [ ] 

#### UC16 - Event Creator Updates Location Name

- [✅] **User Story**: In order to accurately reflect the characteristics or branding of a location, as an event creator,
  I want to update the name of a location.

    - **Success Scenarios**:
        - [✅] S1: Update name of an existing location with a name length between minimum and maximum characters. The
          name should be updated to reflect the new valid name provided. This applies to names like "Beautiful Venue", "
          Loc", and "Spacious and Elegant Event Space".

    - **Failure Scenarios**:
        - [✅] F1: Update name of an existing location with a name length of 0 characters. The system should return a
          failure message indicating that the name cannot be blank.
        - [✅] F2: Update name of an existing location with a name length less than the minimum required characters. The
          system should return a failure message indicating that the name is too short.
        - [✅] F3: Update name of an existing location with a name length more than the maximum allowed characters. The
          system should return a failure message indicating that the name is too long.
        - [✅] F4: Update name of an existing location with a null name. The system should return a failure message
          indicating that the name cannot be null.

#### UC18 - Event Creator Sets Maximum Number of Guests for a Location

- [✅] **User Story**: In order to comply with safety regulations and manage event capacity effectively, as an event
  creator, I want to set the maximum number of guests allowed in a location.

    - **Success Scenarios**:
        - [✅] S1: Given an existing location, when the owner sets the maximum number of guests, then the maximum number
          of guests is set to the selected value. Valid values include 5, 10, 25, and 50.

    - **Failure Scenarios**:
        - [✅] F1: Given an existing location, when the owner sets the number of maximum guests to a number less than the
          minimum required, then a failure message is provided explaining the minimum number of guests. This ensures
          compliance with safety or regulatory minimums.
        - [✅] F2: Given an existing location, when the owner sets the number of maximum guests to a number greater than
          the maximum allowed, then a failure message is provided explaining the maximum number of guests. This ensures
          compliance with safety or regulatory maximums.

#### UC19 - Event Creator Sets Availability Time Span for a Location

- [✅] **User Story**: In order to manage when a location can be booked for events, as an event creator, I want to set
  the availability time span for a location.

    - **Success Scenarios**:
        - [✅] S1: Given an existing location, when the owner sets the availability time span for the future, then the
          availability time span should be updated successfully. The new time span should reflect the future dates set
          by the owner.

    - **Failure Scenarios**:
        - [✅] F1: Given an existing location, when the owner sets the availability time span with a start time in the
          past, then the action should fail. A failure message should be returned, indicating that the start time cannot
          be in the past.
        - [✅] F2: Given an existing location with scheduled events, when the owner tries to set an availability time
          span that overlaps with an existing event, then the action should fail. A failure message should be returned,
          indicating that the new availability time span overlaps with a scheduled event.

#### UC20 - Event Creator Sets Location of Event

- [✅] **User Story**: In order to assign a specific venue to an event, as an event creator, I want to set the location
  of an event.

    - **Success Scenarios**:
        - [✅] S1: Event creator sets the location of an event to a valid location. The location is successfully assigned
          to the event, and the event is added to the list of events at that location.

    - **Failure Scenarios**:
        - [✅] F1: Event creator sets the location of an event to a location that is already associated with another
          active event. The system should return a failure message as a location cannot host multiple events at the same
          time.
        - [✅] F2: Event creator sets the location of an event to a location that is not available during the event's
          scheduled time. The system should return a failure message indicating that the location is unavailable during
          the requested time.

#### UC21 - Guest Requests to Join Private Event

- [✅] **User Story**: In order to attend private events, as a registered guest, I want to request to join a private
  event providing a valid reason.

    - **Success Scenarios**:
        - [✅] S1: Given an existing event with ID, and the event status is active, and the event is private, and a
          registered guest with ID provides a valid reason, and the current number of registered guests is less than the
          maximum number of allowed guests, and the event has not yet started, i.e., before the start time, when the
          guest requests to join the private event, then the event should register that the guest intends to
          participate.

    - **Failure Scenarios**:
        - [✅] F1: Given an existing valid event with ID, and the event status is draft, ready, or cancelled, and a
          registered guest with ID, and the event is private, when the guest chooses to participate in the event, then
          the request is rejected, and a failure message is provided explaining that only active events can be joined.
        - [✅] F2: Given an existing valid event with ID, and the event status is active, and a registered guest with ID,
          and the current number of registered guests is equal to the maximum number of allowed guests, and the event is
          private, when the guest chooses to participate in the event, then the request is rejected, and a failure
          message is provided explaining that there is no more room.
        - [✅] F3: Given an existing valid event with ID, and the event start time is in the past, when the guest chooses
          to participate in the event, then the request is rejected, and a failure message is provided explaining that
          only future events can be participated.
        - [✅] F4: Given an existing valid event with ID, and a registered guest with ID, and the event is private, and
          the guest provides a non-valid reason, when the guest chooses to participate in the event, then the request is
          rejected, and a failure message is provided explaining that the reason is not valid.
        - [✅] F5: Given an existing valid event with ID, and a registered guest with ID, and the guest is already a
          participant at the event, when the guest chooses to participate in the event again, then the request is
          rejected, and a failure message is provided explaining that a guest cannot occupy two slots at an event.

#### UC22 - Event Creator Approves Join Request for Private Event

- [✅] **User Story**: In order to control attendance to a private event, as an event creator, I want to be able to
  approve join requests from guests.

    - **Success Scenarios**:
        - [✅] S1: Given an event creator, a valid event, and a valid user, when the event creator approves the join
          request, then the join request is approved. This includes cases where the event is active, private, has
          available spots, and is scheduled for the future.

    - **Failure Scenarios**:
        - [✅] F1: Given an event creator, a valid event, and a valid user, when the event creator attempts to approve a
          join request for an event that is not in an active status (draft, ready, or cancelled), then the request is
          rejected, and a failure message is provided explaining that only active events can accept new join requests.
        - [✅] F2: Given an event creator, a valid event, and a valid user, when the event creator attempts to approve a
          join request for an event that is full (the number of confirmed participants equals the maximum), then the
          request is rejected, and a failure message is provided explaining that there is no more room.
        - [✅] F3: Given an event creator, a valid event, and a valid user, when the event creator attempts to approve a
          join request for an event whose time span is in the past, then the request is rejected, and a failure message
          is provided explaining that past events cannot accept new join requests.
        - [✅] F4: Given an event creator, a valid event, and a valid user, when the event creator attempts to approve a
          join request that does not exist (the guest did not request to join or the request was previously resolved),
          then the request is rejected, and a failure message is provided.
        - [✅] F5: Given an event creator, a valid event, and a valid user, when the event creator approves a join
          request, but the event is private and the user is not invited, then the request is rejected, and a failure
          message is provided explaining that non-invited guests cannot join private events.
        - [✅] F6: Given an event creator, a valid event, and a valid user, when the event creator attempts to approve a
          join request for an event that is already full, then the request is rejected, and a failure message is
          provided explaining that the event cannot accept more participants.

#### UC23 - Event Creator Declines Join Request

- [✅] **User Story**: In order to manage who can attend a private event, as an event creator, I want to be able to
  decline join requests from guests.

    - **Success Scenarios**:
        - [✅] S1: Given an event creator, a valid event, and a valid user, when the event creator declines the join
          request, then the join request is declined. The guest’s status for the event should be updated to indicate
          they were declined.

    - **Failure Scenarios**:
        - [✅] F1: Given an event creator, a valid event, and a valid user, when the event creator attempts to decline a
          join request that is not in a pending status, then the action should fail. A failure message is provided
          indicating that the join request was not pending.
        - [✅] F2: Given an event creator, a valid event, and a valid user, when the event creator attempts to decline a
          join request for an event whose time span is in the past, then the action should fail. A failure message is
          provided indicating that the event time span is in the past.
        - [✅] F4: Given an event creator, a valid event, and a valid user, when the event creator attempts to decline a
          join request that does not exist (either because it was never made or has already been resolved), then the
          action should fail. A failure message is provided indicating that the join request was not found.

#### UC24 - Event Creator Cancels Event

- [❌] **User Story**: In order to cancel an event, as an event creator, I want to be able to cancel an event.

    - **Success Scenarios**:
        - [❌] S1: Given an event creator, a valid event, and the event is not cancelled, when the event creator cancels
          the event, then the event is cancelled. The status of the event should be updated to indicate that it is
          cancelled.

    - **Failure Scenarios**:
        - [❌] F1: Given an event creator, a valid event, and the event is already cancelled, when the event creator
          attempts to cancel the event, then the action should fail. A failure message is provided indicating that the
          event is already cancelled.

#### UC25 - Event Creator Deletes Event

- [❌] **User Story**: In order to remove an event from the system, as an event creator, I want to be able to delete an
  event.

    - **Success Scenarios**:
        - [❌] S1: Given an event creator, a valid event, and the event is not cancelled, when the event creator deletes
          the event, then the event is deleted. The event should be removed from the system.

    - **Failure Scenarios**:
        - [❌] F1: Given an event creator, a valid event, and the event is already cancelled, when the event creator
          attempts to delete the event, then the action should fail. A failure message is provided indicating that the
          event is already cancelled.

